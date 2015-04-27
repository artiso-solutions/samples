using System;
using System.Configuration;
using System.IO;
using System.Runtime.Remoting.Channels;
using System.Windows.Input;
using Dashboard.Shell.Properties;

namespace Dashboard.Shell
{
   using System.Windows.Navigation;

   public class UpdateCommand : ICommand
   {
      public event EventHandler CanExecuteChanged;

      public bool CanExecute(object parameter)
      {
         return true;
      }

      public void Execute(object parameter)
      {
         DirectoryInfo source;
         var component = parameter as string;

         switch (component)
         {
            case "Service":
               source = new DirectoryInfo(Settings.Default.ServiceUpdateSource);
               source.CopyTo(Path.Combine(Settings.Default.ServiceUpdateTarget, "V2"));
               break;
            case "ClientA":
               source = new DirectoryInfo(Settings.Default.ClientUpdateSource);
               source.CopyTo(Path.Combine(Settings.Default.ClientAUpdateTarget, "V2"));
               break;
            case "ClientB":
               source = new DirectoryInfo(Settings.Default.ClientUpdateSource);
               source.CopyTo(Path.Combine(Settings.Default.ClientBUpdateTarget, "V2"));
               break;
         }
      }
   }
}
