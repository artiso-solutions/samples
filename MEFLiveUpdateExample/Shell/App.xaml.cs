using System.IO;
using System.Windows;

namespace Shell
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        private Bootstrapper bootstrapper;

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
