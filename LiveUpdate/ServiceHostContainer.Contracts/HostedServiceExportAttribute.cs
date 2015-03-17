namespace ServiceHostContainer.Contracts
{
   using System;
   using System.ComponentModel.Composition;
   using System.Reflection;

   [MetadataAttribute]
   [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
   public class HostedServiceExportAttribute : ExportAttribute, IHostedServiceMetadata
   {
      public string ServiceName { get; private set; }

      public string Version { get; private set; }

      public int Order { get; private set; }

      public HostedServiceExportAttribute(Type serviceType, string serviceName, int order)
         : base(typeof(IHostedService))
      {
         this.ServiceName = serviceName;
         this.Order = order;
         this.Version = Assembly.GetAssembly(serviceType).GetName().Version.ToString();
      }
   }
}