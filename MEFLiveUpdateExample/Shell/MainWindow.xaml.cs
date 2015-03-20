using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows.Controls;
using Contracts;

namespace Shell
{
   using System.Collections.ObjectModel;
   using System.Reflection;
   using System.Windows;
   using System.Windows.Threading;

   /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    [Export]
    public partial class MainWindow
    {
        private IEnumerable<Lazy<IMainControl, IMetaData>> contentControls;

        private ICollection<Assembly> excludedAssemblies;

        public MainWindow()
        {
            InitializeComponent();
            
            excludedAssemblies = new Collection<Assembly>();
            Dispatcher.UnhandledException += DispatcherOnUnhandledException;
        }

        private void DispatcherOnUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs dispatcherUnhandledExceptionEventArgs)
        {
           dispatcherUnhandledExceptionEventArgs.Handled = true;
           this.excludedAssemblies.Add(dispatcherUnhandledExceptionEventArgs.Exception.TargetSite.DeclaringType.Assembly);

           MessageBox.Show("UnhandledException occured. Try to load other version.", "Error!", MessageBoxButton.OK);

           LoadLatestControl(ContentControls);
        }

        [ImportMany(typeof(IMainControl), AllowRecomposition = true)]
        public IEnumerable<Lazy<IMainControl, IMetaData>> ContentControls
        {
            get
            {
                return this.contentControls;
            }
            set
            {
                this.contentControls = value;
                this.LoadLatestControl(value);
            }
        }

        private void LoadLatestControl(IEnumerable<Lazy<IMainControl, IMetaData>> controls)
        {
           var controlsOrderedByVersion = controls.OrderByDescending(c => c.Metadata.Version);

           this.Dispatcher.Invoke(() => { var content = controlsOrderedByVersion.Select(c => c.Value).FirstOrDefault(c => !this.excludedAssemblies.Contains(c.GetType().Assembly)); this.Content = content; });
        }
    }
}
