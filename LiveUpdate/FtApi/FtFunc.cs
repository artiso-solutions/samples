using FtApi.Enums;

namespace FtApi
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Threading;

    using log4net;

    public enum CounterMode
    {
        Normal,
        Inverted,
    }

    public static class FtFunc
    {
    private static ILog logger = LogManager.GetLogger(typeof(FtFunc));

        private static AsyncMotorControl motorController = new AsyncMotorControl();

        public static ControllerConfiguration InitializeController(string comPortName, int deviceId)
        {
            logger.Info("Initialize controller");
            uint errCode = DLL.InitLib();
            if (errCode != FtConsts.Success)
            {
                throw new InitializationException("Failed to initialize library.");
            }

            logger.DebugFormat("Open com device for port {0}", comPortName);
            var ftHandle = DLL.OpenComDevice(comPortName, 38400, ref errCode);
            if (errCode != FtConsts.Success)
            {
                throw new InitializationException(string.Format("Failed to open com port {0}.", comPortName));
            }

            errCode = DLL.StartTransferArea(ftHandle);
            if (errCode != FtConsts.Success)
            {
                throw new InitializationException("Failed to start transfer area.");
            }

            return new ControllerConfiguration
                       {
                           FtHandle = ftHandle,
                           DeviceId = deviceId
                       };
        }

        private static void DoNothingHandler(object sender, EventArgs args)
        {
        }

        public static void CloseController(ControllerConfiguration configuration)
        {
            motorController.MotorStopped -= DoNothingHandler;

            DLL.StopTransferArea(configuration.FtHandle);

            logger.Info("Close controller");
            DLL.CloseDevice(configuration.FtHandle);

            DLL.CloseLib();
        }
        
        public static void SetOutValue(ControllerConfiguration configuration, int portNumber, int value)
        {
            DLL.SetPwmValues(configuration.FtHandle, configuration.DeviceId, portNumber, value);
        }
       
        public static void StartAlarmLight(ControllerConfiguration configuration, int portNumber, int timeout, double speed = 2.0)
        {
            var threadStart = new ThreadStart((Action)(() => 
            {
                logger.Debug("start alarm light");

                speed = Math.Max(0.0, Math.Min(10.0, speed));
                var endTime = DateTime.Now.AddMilliseconds(timeout);
                while (DateTime.Now < endTime)
                {
                    for (double i = 0.0; i < 10.0; i += speed)
                    {
                        var value = (int)(i * (512 / 10.0));
                        //logger.DebugFormat("alarm value = {0}", value);
                        DLL.SetPwmValues(configuration.FtHandle, configuration.DeviceId, portNumber, value);
                        Thread.Sleep(50);
                    }

                    for (double i = 10.0; i >= 0.0; i -= speed)
                    {
                        var value = (int)(i * (512 / 10.0));
                        //logger.DebugFormat("alarm value = {0}", value);
                        DLL.SetPwmValues(configuration.FtHandle, configuration.DeviceId, portNumber, value);
                        Thread.Sleep(50);
                    }
                }

                DLL.SetPwmValues(configuration.FtHandle, configuration.DeviceId, portNumber, 0);
            }));
            var thread = new Thread(threadStart);
            thread.Start();
        }

        public static void StartMotor(ControllerConfiguration configuration, int motorNumber, MotorDirection direction, int speed)
        {
            logger.Info("Start motor with values ");

            DLL.StartMotorExCmd(configuration.FtHandle, configuration.DeviceId, motorNumber, speed, (int)direction, motorNumber, (int)direction, 0);
        }

    public static void StartMotor(ControllerConfiguration configuration, int motorNumber, int duty_p, int duty_m, bool brake)
    {
      DLL.SetMotorValues(configuration.FtHandle, configuration.DeviceId, motorNumber, duty_p, duty_m, brake);
    }

    public static void StopMotor(ControllerConfiguration configuration, int motorNumber)
        {
            DLL.SetMotorConfig(configuration.FtHandle, configuration.DeviceId, motorNumber*2, FtConsts.MotorOff);
            DLL.SetMotorConfig(configuration.FtHandle, configuration.DeviceId, motorNumber*2 + 1, FtConsts.MotorOff);
            DLL.SetPwmValues(configuration.FtHandle, configuration.DeviceId, motorNumber*2, 0);
            DLL.SetPwmValues(configuration.FtHandle, configuration.DeviceId, motorNumber*2 + 1, 0);
        }

        public static void SetIoMode(ControllerConfiguration configuration, int ioId, IoType ioType)
        {
            switch (ioType)
            {
                case IoType.PushButton:
                    DLL.SetUniConfig(configuration.FtHandle, configuration.DeviceId, ioId, FtConsts.InputModeResistance, FtConsts.InputIsDigital);
                    break;
                case IoType.TrackSensor:
                    DLL.SetUniConfig(configuration.FtHandle, configuration.DeviceId, ioId, FtConsts.InputModeVoltage, FtConsts.InputIsAnalog);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("ioType");
            }
        }

        public static bool GetDigitalIo(ControllerConfiguration configuration, int ioId)
        {
            short value = 0;
            bool overrun = false;

            DLL.GetUniInValue(configuration.FtHandle, configuration.DeviceId, ioId, ref value, ref overrun);

            return value == 1;
        }

        public static short GetAnalogIo(ControllerConfiguration configuration, int ioId)
        {
            short value = 0;
            bool overrun = false;

            DLL.GetUniInValue(configuration.FtHandle, configuration.DeviceId, ioId, ref value, ref overrun);

            return value;
        }

        public static bool WaitForTrackSensorChanged(ControllerConfiguration configuration, int ioId, int threshold, int timeout = 30000)
        {
            logger.Debug("wait for track sensor changed");

            var initialValue = GetAnalogIo(configuration, ioId);
            var initialIsLess = initialValue < threshold;

            logger.DebugFormat("id = {0} initial value = {1} threshold = {2}", ioId, initialValue, threshold);

            var timeoutTime = DateTime.Now.AddMilliseconds(timeout);
            while(DateTime.Now < timeoutTime)
            {
                var currentValue = GetAnalogIo(configuration, ioId);
                // logger.DebugFormat("id = {0} current value = {1}", ioId, currentValue);

                if (initialIsLess)
                {
                    if (currentValue > threshold)
                    {
                        logger.Debug("threshold reached");
                        return true;
                    }
                }
                else
                {
                    if (currentValue < threshold)
                    {
                        logger.Debug("threshold reached");
                        return true;
                    }
                }

                Thread.Sleep(10);
            }

            return false;
        }

        public static void StartMotorSynchron(ControllerConfiguration configuration, int masterNumber, MotorDirection masterDirection, int slaveNumber, MotorDirection slaveDirection, int speed)
        {
            DLL.StartMotorExCmd(configuration.FtHandle, configuration.DeviceId, masterNumber, speed, (int)masterDirection, slaveNumber, (int)slaveDirection, 0);
        }

        public static void RunMotorDistanceSynchron(ControllerConfiguration configuration, int masterNumber, MotorDirection masterDirection, int slaveNumber, MotorDirection slaveDirection, int speed, int distance)
        {
            ////motorController.StartMotors((uint)masterNumber, (uint)slaveNumber);
            DLL.StartMotorExCmd(configuration.FtHandle, configuration.DeviceId, masterNumber, speed, (int)masterDirection, slaveNumber, (int)slaveDirection, distance);
        }

        public static void RunMotorDistance(ControllerConfiguration configuration, int motorNumber, MotorDirection direction, int speed, int distance)
        {
            ////motorController.StartMotors((uint)motorNumber);
            DLL.StartMotorExCmd(configuration.FtHandle, configuration.DeviceId, motorNumber, speed, (int)direction, motorNumber, (int)direction, distance);
        }

        public static int GetCounterValue(ControllerConfiguration configuration, int counterNumber)
        {
            ushort count = 0;
            ushort val = 0;
            DLL.GetInCounterValue(configuration.FtHandle, configuration.DeviceId, counterNumber, ref count, ref val);
            return (int)count;
        }

        public static CounterMode GetCounterMode(ControllerConfiguration configuration, int counterNumber)
        {
            ushort count = 0;
            ushort mode = 0;
            DLL.GetInCounterValue(configuration.FtHandle, configuration.DeviceId, counterNumber, ref count, ref mode);
            return mode == 0 ? CounterMode.Normal : CounterMode.Inverted;
        }

        public static void StartResetCounter(ControllerConfiguration configuration, int counterNumber)
        {
            DLL.StartCounterReset(configuration.FtHandle, configuration.DeviceId, counterNumber);
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void CounterResetCallbackDelegate(uint deviceId, uint counterNumber);

        private static AutoResetEvent counterResetEvent = new AutoResetEvent(false);

        public static void ResetCounter(ControllerConfiguration configuration, int counterNumber)
        {
            logger.Debug("start reset counter...");

            counterResetEvent.Reset();

            DLL.SetCBCounterResetted((CounterResetCallbackDelegate)CounterResettedCallback);
            DLL.StartCounterReset(configuration.FtHandle, configuration.DeviceId, counterNumber);

            if (!counterResetEvent.WaitOne(30000))
            {
                logger.Error("counter has not resetted in 30 sec.");
            }
            else
            {
                logger.Debug("counter is resetted");
            }

            DLL.SetCBCounterResetted((CounterResetCallbackDelegate)null);

            logger.Debug("start reset counter finished");
        }

        private static void CounterResettedCallback(uint deviceId, uint counterNumber)
        {
            logger.Debug("counter resetted callback entered");
            counterResetEvent.Set();
        }

        public static void WaitForMotorsStopped(params uint[] motorNumbers)
        {
            ////EventHandler<EventArgs> handler = null;
            ////var waitHandle = new AutoResetEvent(false);

            ////handler = (sender, args) =>
            ////    {
            ////        if (motorController.RunningMotors.Intersect(motorNumbers).Any())
            ////        {
            ////            return;
            ////        }

            ////        waitHandle.Set();

            ////        motorController.MotorStopped -= handler;
            ////    };

            ////motorController.MotorStopped += handler;

            ////waitHandle.WaitOne(TimeSpan.FromSeconds(10));
        }

        public static void ShowMessage(ControllerConfiguration configuration, string message)
        {
            DLL.SetRoboTxMessage(configuration.FtHandle, configuration.DeviceId, message);
        }
        
        public class AsyncMotorControl
        {
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            public delegate void CallbackDelegate(uint deviceId, uint motorNumber);

            public AsyncMotorControl()
            {
                this.RunningMotors = new List<uint>();
                DLL.SetCBMotorExReached((CallbackDelegate)this.MotorMotionFinished);
            }

            public List<uint> RunningMotors { get; private set; }

            public void StartMotors(params uint[] motorNumbers)
            {
                this.RunningMotors.AddRange(motorNumbers.Distinct().Where(number => !this.RunningMotors.Contains(number)).ToList());
            }

            private void MotorMotionFinished(uint deviceId, uint motorNumber)
            {
                if (this.RunningMotors.Contains(motorNumber))
                {
                    this.RunningMotors.Remove(motorNumber);
                }

                if (this.MotorStopped != null)
                {
                    this.MotorStopped(this, new EventArgs());
                }
            }

            public event EventHandler<EventArgs> MotorStopped;
        }
    }
}