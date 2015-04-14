using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Threading;
using ClientContracts;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;

namespace Shell
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    [Export]
    public partial class MainWindow : Window
    {
        private IEnumerable<Lazy<IMainControl, IMetaData>> contentControls;

        private ICollection<Assembly> excludedAssemblies;
        private TelemetryClient tc = new TelemetryClient();

        public MainWindow()
        {
            InitializeComponent();
            excludedAssemblies = new Collection<Assembly>();
            Dispatcher.UnhandledException += DispatcherOnUnhandledException;
            TelemetryConfiguration.Active.ContextInitializers.Add(new TelementryInitializer());
        }

        private void DispatcherOnUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs dispatcherUnhandledExceptionEventArgs)
        {
            dispatcherUnhandledExceptionEventArgs.Handled = true;
            tc.TrackException(dispatcherUnhandledExceptionEventArgs.Exception);
            tc.TrackEvent("Fallback");
        
            this.excludedAssemblies.Add(dispatcherUnhandledExceptionEventArgs.Exception.TargetSite.DeclaringType.Assembly);

            string message = "Something unexpected happened and we restored the previous version to savely continue operation! The development team has been notified and will provide a fixed version asap.";

            ((MainWindowViewModel)this.DataContext).ErrorMessage = message;
            var wcfClient = new WcfClient();
            wcfClient.NotifyFallback(GetClientName(), dispatcherUnhandledExceptionEventArgs.Exception);
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
            //var latestControl = controls.FirstOrDefault(c => c.Metadata.Version == controls.Max(a => a.Metadata.Version));
            //if (latestControl != null)
            {
                //if (this.Dispatcher.CheckAccess())
                //{
                //    this.Content = latestControl.Value as Control;
                //}
                //else
                {
                    var controlsOrderedByVersion = controls.OrderByDescending(c => c.Metadata.Version);
                    this.Dispatcher.Invoke(() =>
                    {
                        var content =
                            controlsOrderedByVersion.Select(c => c.Value)
                                .FirstOrDefault(c => !this.excludedAssemblies.Contains(c.GetType().Assembly));
                        this.Container.Content = content;
                        tc.TrackTrace(String.Format("Loaded version {0} {1}", Content.GetType().FullName,
                            Content.GetType().Assembly.FullName));
                        tc.TrackEvent("VersionChange");
                        var wcfClient = new WcfClient();
                        string version = "v" + controlsOrderedByVersion.FirstOrDefault(c => !this.excludedAssemblies.Contains(c.Value.GetType().Assembly)).Metadata.Version.ToString();

                        string clientName = GetClientName();
                        wcfClient.DashboardUpdatedVersion(clientName, version);
                    });
                   
                }
            }
        }

        private string GetClientName()
        {
            if (Assembly.GetExecutingAssembly().Location.ToLower().Contains("clientb"))
            {
                return "ClientB";
            }
            else
            {
                return "ClientA";
            }
        }
    }
}
