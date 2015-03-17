using ClientContracts;
using MefContrib.Hosting;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Shell
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
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
