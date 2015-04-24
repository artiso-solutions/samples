using System;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Threading;

namespace Shell
{
   /// <summary>
   /// basic boot strapper used to build MEF container
   /// </summary>
   public class Bootstrapper
   {
      private FileSystemWatcher fileSystemWatcher;

      private AggregateCatalog aggregateCatalog;

      /// <summary>
      /// Initializes a new instance of the <see cref="Bootstrapper"/> class.
      /// </summary>
      /// <param name="applicationPath">The application path.</param>
      /// <param name="watchDirectory">
      /// if set to <c>true</c> [watch directory] the directory 
      /// will be watched and newly added assemblies loaded automatically.
      /// </param>
      public Bootstrapper(string applicationPath, bool watchDirectory)
      {
         this.Container = ComposeMefContainer(applicationPath);

         if (watchDirectory)
         {
            this.fileSystemWatcher = new FileSystemWatcher(applicationPath) { EnableRaisingEvents = true };
            this.fileSystemWatcher.Changed += this.FilesSystemChanged;
         }
      }

      public CompositionContainer Container { get; private set; }

      private CompositionContainer ComposeMefContainer(string applicationPath)
      {
         this.aggregateCatalog = new AggregateCatalog();
         var appCatalog = new ApplicationCatalog();

         this.BuildDirectoryCatalog(this.aggregateCatalog, applicationPath);
         this.aggregateCatalog.Catalogs.Add(appCatalog);

         var container = new CompositionContainer(this.aggregateCatalog, true);
         return container;
      }

      private void FilesSystemChanged(object sender, FileSystemEventArgs eventArgs)
      {
         // when something was chagned in directory, refresh directory catalog
         Thread.Sleep(500);
         if (eventArgs.ChangeType == WatcherChangeTypes.Changed && Directory.Exists(eventArgs.FullPath))
         {
            var existingDirectoryCatalog =
                this.aggregateCatalog.Catalogs.OfType<DirectoryCatalog>()
                    .SingleOrDefault(catalog => string.Equals(catalog.FullPath, eventArgs.FullPath, StringComparison.InvariantCultureIgnoreCase));

            if (existingDirectoryCatalog != null)
            {
               existingDirectoryCatalog.Refresh();
            }
            else
            {
               this.aggregateCatalog.Catalogs.Add(new DirectoryCatalog(eventArgs.FullPath));
            }
         }
      }

      private void BuildDirectoryCatalog(AggregateCatalog aggregateCatalog, string applicationPath)
      {
         var directoryInfo = new DirectoryInfo(applicationPath);
         if (directoryInfo.Exists)
         {
            var subDirectories = directoryInfo.GetDirectories().Reverse();
            foreach (var subDirectory in subDirectories)
            {
               aggregateCatalog.Catalogs.Add(new DirectoryCatalog(subDirectory.FullName));
            }
         }
      }
   }
}