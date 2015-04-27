using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Shell
{
   public static class CopyDirectoryExtension
   {
      public static DirectoryInfo CopyTo(this DirectoryInfo sourceDir, string destinationPath, bool overwrite = false)
      {
         var sourcePath = sourceDir.FullName;

         var destination = new DirectoryInfo(destinationPath);
         if (destination.Exists)
         {
            return destination;
         }

         destination.Create();

         if (!sourceDir.Exists)
         {
            return destination;
         }

         foreach (var sourceSubDirPath in Directory.EnumerateDirectories(sourcePath, "*", SearchOption.AllDirectories))
            Directory.CreateDirectory(sourceSubDirPath.Replace(sourcePath, destinationPath));

         foreach (var file in Directory.EnumerateFiles(sourcePath, "*", SearchOption.AllDirectories))
            File.Copy(file, file.Replace(sourcePath, destinationPath), overwrite);

         return destination;
      }
   }
}
