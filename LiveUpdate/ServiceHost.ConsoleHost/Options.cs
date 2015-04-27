namespace ConsoleHost
{
   using CommandLine;

   public class Options
   {
      [Option("UseFischerTechnikController", DefaultValue = false, Required = false)]
      public bool UseFischerTechnikController { get; set; }

      [Option("DashboardComputerName", DefaultValue = "localhost", Required = false)]
      public string DashboardComputerName { get; set; }
   }
}