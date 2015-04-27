namespace Shell
{
   using System;

   using Microsoft.ApplicationInsights.DataContracts;
   using Microsoft.ApplicationInsights.Extensibility;

   public class TelementryInitializer : IContextInitializer
   {
      public void Initialize(TelemetryContext context)
      {
         context.User.Id = Environment.UserName;
         context.Session.Id = DateTime.Now.ToFileTime().ToString();
         context.Session.IsNewSession = true;
         context.Device.OperatingSystem = Environment.OSVersion.VersionString;
      }
   }
}
