// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="">
//   
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ConsoleHost
{
   using System;
   using System.Linq;

   using CommandLine;

   using FischerTechnikService;

   using log4net;

   using ServiceHostContainer;

   /// <summary>
   /// The program.
   /// </summary>
   internal class Program
   {
      #region Methods

      private static void Main(string[] args)
      {
         ILog logger = LogManager.GetLogger(typeof(Program));
         logger.Info("Starting console host...");

         IFischerTechnikLogic fischerTechnikLogic;

         var options = new Options();
         Parser.Default.ParseArguments(args, options);

         if (options.UseFischerTechnikController)
         {
            logger.Info("Using Fischer Technik Logic...");
            fischerTechnikLogic = new FischerTechnikLogic();
         }
         else
         {
            logger.Info("Using Fischer Technik Test Logic...");
            fischerTechnikLogic = new FischerTechnikTestLogic();
         }

         var dashboardComputerName = options.DashboardComputerName;


         fischerTechnikLogic.StartMotor();

         var container = new ServiceHostContainer();
         container.Start(fischerTechnikLogic, dashboardComputerName);

         Console.WriteLine("Press <enter> to exit...");
         Console.ReadLine();
      }

      #endregion
   }
}