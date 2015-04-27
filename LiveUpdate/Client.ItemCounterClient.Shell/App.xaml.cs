namespace Shell
{
   using System.IO;
   using System.Windows;

   /// <summary>
   /// Interaction logic for App.xaml
   /// </summary>
   public partial class App : Application
   {
      private readonly Bootstrapper bootstrapper;
      public App()
      {
         var applicationPath = Path.GetDirectoryName(this.GetType().Assembly.Location);
         this.bootstrapper = new Bootstrapper(applicationPath, true);
      }

      protected override void OnStartup(StartupEventArgs e)
      {
         base.OnStartup(e);

         this.MainWindow = this.bootstrapper.Container.GetExportedValue<MainWindow>();
         this.MainWindow.Show();
      }
   }
}
