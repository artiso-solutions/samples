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
   using Dashboard.Client;

   using log4net;

   /// <summary>
   /// Interaction logic for MainWindow.xaml
   /// </summary>
   [Export]
   public partial class MainWindow : Window
   {
      private IEnumerable<Lazy<IMainControl, IMetaData>> contentControls;

      private readonly ICollection<Assembly> excludedAssemblies;
      private readonly TelemetryClient telemetryClient = new TelemetryClient();

      private readonly ILog logger;

      private readonly DashboardClient dashboardClient;

      public MainWindow()
      {
         InitializeComponent();
         logger = LogManager.GetLogger(typeof(MainWindow));
         dashboardClient = new DashboardClient(logger);
         excludedAssemblies = new Collection<Assembly>();
         Dispatcher.UnhandledException += DispatcherOnUnhandledException;
         TelemetryConfiguration.Active.ContextInitializers.Add(new TelementryInitializer());
      }

      private void DispatcherOnUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs dispatcherUnhandledExceptionEventArgs)
      {
         dispatcherUnhandledExceptionEventArgs.Handled = true;
         telemetryClient.TrackException(dispatcherUnhandledExceptionEventArgs.Exception);
         telemetryClient.TrackEvent("Fallback");

         this.excludedAssemblies.Add(dispatcherUnhandledExceptionEventArgs.Exception.TargetSite.DeclaringType.Assembly);

         const string Message = "Something unexpected happened and we restored the previous version to savely continue operation! The development team has been notified and will provide a fixed version asap.";

         ((MainWindowViewModel)this.DataContext).ErrorMessage = Message;

         dashboardClient.NotifyFallback(GetClientName(), dispatcherUnhandledExceptionEventArgs.Exception);
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
         this.Dispatcher.Invoke(() =>
         {
            var content =
                controlsOrderedByVersion.Select(c => c.Value)
                    .FirstOrDefault(c => !this.excludedAssemblies.Contains(c.GetType().Assembly));
            this.Container.Content = content;
            telemetryClient.TrackTrace(String.Format("Loaded version {0} {1}", Content.GetType().FullName,
                Content.GetType().Assembly.FullName));
            telemetryClient.TrackEvent("VersionChange");
            string version = "v" + controlsOrderedByVersion.FirstOrDefault(c => !this.excludedAssemblies.Contains(c.Value.GetType().Assembly)).Metadata.Version.ToString();

            string clientName = GetClientName();
            dashboardClient.DashboardUpdatedVersion(clientName, version);
         });
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
