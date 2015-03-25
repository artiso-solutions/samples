// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="">
//   
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ConsoleHost
{
   using System;

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

         if (args.Length == 0)
         {
            logger.Info("Using Fischer Technik Test Logic...");
            fischerTechnikLogic = new FischerTechnikTestLogic();
         }
         else
         {
            logger.Info("Using Fischer Technik Logic...");
            fischerTechnikLogic = new FischerTechnikLogic();
         }

         fischerTechnikLogic.StartMotor();

         var container = new ServiceHostContainer();
         container.Start(fischerTechnikLogic);

         Console.WriteLine("Press <enter> to exit...");
         Console.ReadLine();
      }

      #endregion
   }
}