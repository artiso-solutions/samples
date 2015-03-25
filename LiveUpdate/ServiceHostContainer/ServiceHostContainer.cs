using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using log4net;
using ServiceHostContainer.Configuration;
using ServiceHostContainer.Contracts;

namespace ServiceHostContainer
{
   using System.ComponentModel.Composition;
   using System.ComponentModel.Composition.Primitives;
   using System.IO;
   using System.Reflection;
   using System.Text;

   using FischerTechnikService;

   using MefContrib.Hosting;

   public class ServiceHostContainer
   {

      private readonly Dictionary<string, ServiceHost> serviceHosts = new Dictionary<string, ServiceHost>();

      private CompositionContainer compositionContainer;

      private readonly ILog logger = LogManager.GetLogger(typeof(ServiceHostContainer));

      private string applicationPath;

      private AggregateCatalog aggregateCatalog;

      private IEnumerable<Lazy<IHostedService, IHostedServiceMetadata>> services;

      private ServiceHostContainerConfiguration configuration;

      private ServiceHost dispatcherHost;

      private ServiceHost fischerTechnikServiceHost;

      public State ServiceHostState { get; set; }

      public void Start(IFischerTechnikLogic fischerTechnikLogic)
      {
            logger.Info("Starting service host...");
            if (this.ServiceHostState != State.Stopped)
         {
            var message = string.Format("Cannot start service host when in state {0}.", this.ServiceHostState);
            logger.Warn(message);
            throw new NotSupportedException(message);
         }
         this.ServiceHostState = State.Starting;


         this.applicationPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
         logger.Info(string.Format("Searching for services in {0}", applicationPath));
         FileSystemWatcher fs = new FileSystemWatcher(applicationPath, "*.dll");
         fs.IncludeSubdirectories = true;
         fs.Changed += Changed;
         fs.Created += Changed;
         fs.EnableRaisingEvents = true;
         configuration = ServiceHostContainerReader.ReadConfiguration();


         this.InitializeCompositionContainer();

         string dispatchName = StartDispatcherService();
         logger.Info(string.Format("Service \"{0}\" started.", dispatchName));

         string fischertechnikName = StartFischerTechnikService(fischerTechnikLogic);
         logger.Info(string.Format("Service \"{0}\" started.", fischertechnikName));
         foreach (var endpoint in fischerTechnikServiceHost.Description.Endpoints)
         {
            logger.Debug(string.Format("    Endpoint: {0} - {1} ({2})", endpoint.Name, endpoint.ListenUri, endpoint.Binding.Name));
         }

         this.ServiceHostState = State.Started;
         logger.Info("Service host started.");
      }

      private string StartFischerTechnikService(IFischerTechnikLogic fischerTechnikLogic)
      {
         var fischerTechnikService = new FischerTechnikService(fischerTechnikLogic);
         fischerTechnikService.StartListening();
         var fischertechnikName = "Fischertechnik";
         fischerTechnikServiceHost = new ServiceHost(fischerTechnikService, configuration.BaseAddresses.Select(a => new Uri(a, fischertechnikName)).ToArray());
         fischerTechnikServiceHost.Open();
         return fischertechnikName;
      }

      private string StartDispatcherService()
      {
         var dispatcherService = new ServiceDispatcher(serviceHosts);
         var dispatchName = "Dispatcher";
         dispatcherHost = new ServiceHost(dispatcherService, configuration.BaseAddresses.Select(a => new Uri(a, dispatchName)).ToArray());
         dispatcherHost.Open();
         return dispatchName;
      }

      [ImportMany(typeof(IHostedService), AllowRecomposition = true)]
      public IEnumerable<Lazy<IHostedService, IHostedServiceMetadata>> Services
      {
         get
         {
            return this.services;
         }
         set
         {
            this.services = value;

            try
            {
               // start services
               foreach (var serviceCreator in services.OrderBy(s => s.Metadata.Order).ToList())
               {
                  this.CreateService(serviceCreator, configuration);
               }
            }
            catch (Exception exp)
            {
               logger.Error("Error during start of services.", exp);
            }
         }
      }
      private void CreateService(Lazy<IHostedService, IHostedServiceMetadata> hostedServiceCreator, ServiceHostContainerConfiguration configuration)
      {
         var serviceName = hostedServiceCreator.Metadata.ServiceName;
         var version = hostedServiceCreator.Metadata.Version;
         var fullName = string.Format("{0}{1}", serviceName, version);

         if (this.serviceHosts.ContainsKey(fullName))
         {
            return;
         }

            logger.Debug(string.Format("Starting service {0}...", fullName));

            try
            {
            var serviceInstance = hostedServiceCreator.Value;

            var host = new ServiceHost(serviceInstance, configuration.BaseAddresses.Select(a => new Uri(a, fullName)).ToArray());
            this.serviceHosts[fullName] = host;

            host.Open();

            serviceInstance.OnStart();
            logger.Info(string.Format("Service \"{0}\" started.", fullName));

            foreach (var endpoint in host.Description.Endpoints)
            {
               logger.Debug(string.Format("    Endpoint: {0} - {1} ({2})", endpoint.Name, endpoint.ListenUri, endpoint.Binding.Name));
            }
         }
         catch (Exception e)
         {
            logger.Error(string.Format("Service \"{0}\" not started.", fullName), e);
         }
      }

      private void Changed(object sender, FileSystemEventArgs e)
      {
         this.BuildRecursiveCatalog();
      }

      private void InitializeCompositionContainer()
      {
         var applicationCatalog = new ApplicationCatalog();

         aggregateCatalog = new AggregateCatalog(applicationCatalog);

         BuildRecursiveCatalog();


         this.compositionContainer = new CompositionContainer(aggregateCatalog, CompositionOptions.DisableSilentRejection | CompositionOptions.IsThreadSafe);
         this.compositionContainer.ComposeParts(this);
      }

      private void BuildRecursiveCatalog()
      {
         var recursive = new RecursiveDirectoryCatalog(applicationPath);

         if (!ShouldReloadCatalogs(recursive, aggregateCatalog.Catalogs.FirstOrDefault()))
         {
            return;
         }

         aggregateCatalog.Catalogs.Clear();
         aggregateCatalog.Catalogs.Add(recursive);
      }

      private static bool ShouldReloadCatalogs(ComposablePartCatalog newLoadedCatalog, ComposablePartCatalog existingCatalog)
      {
         if (existingCatalog == null && newLoadedCatalog != null)
         {
            return true;
         }

         if (newLoadedCatalog == null)
         {
            return false;
         }

         var newExportDef = newLoadedCatalog.Parts.SelectMany(p => p.ExportDefinitions).ToList();
         var existingExportDef = existingCatalog.Parts.SelectMany(p => p.ExportDefinitions).ToList();

         if (BuildCompareString(newExportDef) != BuildCompareString(existingExportDef))
         {
            return true;
         }

         return false;
      }

      private static string BuildCompareString(List<ExportDefinition> newExportDef)
      {
         List<string> keyValueList = new List<string>();
         foreach (var exportDefinition in newExportDef)
         {
            StringBuilder myBuilder = new StringBuilder();
            foreach (var o in exportDefinition.Metadata)
            {
               myBuilder.Append(o.Key);
               myBuilder.Append(o.Value);
            }

            var value = myBuilder.ToString();
            if (!keyValueList.Contains(value))
            {
               keyValueList.Add(value);
            }
         }

         return string.Join(",", keyValueList.ToArray());
      }

      public void Stop()
      {
         logger.Info("Processing stop request...");
         if (this.ServiceHostState != State.Started)
         {
            var message = string.Format("Cannot stop service host when in state {0}.", this.ServiceHostState);
            logger.Warn(message);
            throw new NotSupportedException(message);
         }

         logger.Info("Stopping service host...");
         this.ServiceHostState = State.Stopping;

         foreach (var host in this.serviceHosts.Values)
         {
            ((IHostedService)host.SingletonInstance).OnStop();
         }

         foreach (var host in this.serviceHosts.Values)
         {
            host.Abort();
         }

         dispatcherHost.Abort();
         fischerTechnikServiceHost.Abort();

         this.ServiceHostState = State.Stopped;
         logger.Info("Service host stopped.");
      }
   }
}