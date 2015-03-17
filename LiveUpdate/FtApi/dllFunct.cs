namespace FtApi {
    using System;
    using System.Runtime.InteropServices;
    using System.Text;

    //  error codes from ftMscLib.dll
    public enum FTERR : uint {

        FTLIB_ERR_SUCCESS = 0x00000000
    };

    // Identifiers of the Transfer Area parts
    public enum TA_ID {
        TA_LOCAL = 0,           // Local part of Transfer Area. Corresponds to the device on which  
                                // program is currently running in download (local) mode or to the
                                // remotely controlled device (seen from controlled device not from
                                // controlling device) in online mode.
        TA_EXT_1,               // Extension 1 part of Transfer Area
        TA_EXT_2,               // Extension 2 part of Transfer Area
        TA_EXT_3,               // Extension 3 part of Transfer Area
        TA_EXT_4,               // Extension 4 part of Transfer Area
        TA_EXT_5,               // Extension 5 part of Transfer Area
        TA_EXT_6,               // Extension 6 part of Transfer Area
        TA_EXT_7,               // Extension 7 part of Transfer Area
        TA_EXT_8,               // Extension 8 part of Transfer Area
        TA_N_PARTS              // Number of Transfer Area parts
    };

    // Modes of universal inputs
    public enum InputMode {
        MODE_U = 0,             // mV 
        MODE_R = 1,             // 5 kOhm
        MODE_ULTRASONIC = 3,
        MODE_INVALID
    };


    public class DLL {

        const string dllpath = "ftMscLib.dll";

        const int VERSTRLEN = 25;

        [DllImport(dllpath, EntryPoint = "ftxGetLibVersionStr")]
        static extern uint GetLibVersionStr(StringBuilder sb, uint len);

        [DllImport(dllpath, EntryPoint = "ftxInitLib")]
        public static extern uint InitLib();

        [DllImport(dllpath, EntryPoint = "ftxCloseLib")]
        public static extern uint CloseLib();

        [DllImport(dllpath, EntryPoint = "ftxOpenComDevice")]
        public static extern uint OpenComDevice(string comStr, uint bdr, ref uint err);

        [DllImport(dllpath, EntryPoint = "ftxCloseDevice")]
        public static extern uint CloseDevice(uint fthdl);

        [DllImport(dllpath, EntryPoint = "ftxStartTransferArea")]
        public static extern uint StartTransferArea(uint fthdl);

        [DllImport(dllpath, EntryPoint = "ftxStopTransferArea")]
        public static extern uint StopTransferArea(uint fthdl);

        [DllImport(dllpath, EntryPoint = "SetOutMotorValues")]
        public static extern uint SetMotorValues(uint fthdl, int shmId, int id, int duty_p, int duty_m, bool brake);

        [DllImport(dllpath, EntryPoint = "SetOutPwmValues")]
        public static extern uint SetPwmValues(uint fthdl, int shmId, int id, int duty);

        [DllImport(dllpath, EntryPoint = "SetFtMotorConfig")]
        public static extern uint SetMotorConfig(uint fthdl, int shmId, int id, bool status);

        [DllImport(dllpath, EntryPoint = "SetFtUniConfig")]
        public static extern uint SetUniConfig(uint fthdl, int shmId, int id, int mode, bool status);

        [DllImport(dllpath, EntryPoint = "StartCounterReset")]
        public static extern uint StartCounterReset(uint fthdl, int shmId, int id);

        [DllImport(dllpath, EntryPoint = "SetCBCounterResetted", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint SetCBCounterResetted(MulticastDelegate callback);

        [DllImport(dllpath, EntryPoint = "GetInCounterValue")]
        public static extern unsafe uint GetInCounterValue(uint fthdl, int shmId, int idx, ref ushort count, ref ushort val);

        [DllImport(dllpath, EntryPoint = "GetInIOValue")]
        public static extern unsafe uint GetUniInValue(uint fthdl, int shmId, int idx, ref short val, ref bool overrun);

        [DllImport(dllpath, EntryPoint = "GetInDisplayButtonValue")]
        public static extern unsafe uint GetDisplayButton(uint fthdl, int shmId, ref ushort left, ref ushort right);

        [DllImport(dllpath, EntryPoint = "SetRoboTxMessage")]
        public static extern uint SetRoboTxMessage(uint fthdl, int shmId, string msg);

        [DllImport(dllpath, EntryPoint = "StartMotorExCmd")]
        public static extern uint StartMotorExCmd(uint fthdl, int shmId, int masterId, int duty, int masterDirection, int slaveId, int slaveDirection, int pulseCount);

        [DllImport(dllpath, EntryPoint = "SetCBMotorExReached", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetCBMotorExReached(MulticastDelegate callback);

        //  GetLibVersionStr
        //---------------------------------------------------------------------
        public static string ftLibVersionStr() {
            StringBuilder sb = new StringBuilder(null, VERSTRLEN + 1);
            return ((GetLibVersionStr(sb, VERSTRLEN) != 0) ? sb.ToString() : null);
        }
    }
}

