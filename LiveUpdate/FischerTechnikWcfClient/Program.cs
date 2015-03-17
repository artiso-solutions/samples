using System;

namespace FischerTechnikWcfClient
{
   class Program
   {
      private static void Main(string[] args)
      {
         Console.Write("Enter any key to subscribe to service");
         Console.Read();

         var client = new FischerTechnikClient();
         client.ConnectToService();

         Console.Read();
         client.Dispose();
      }
   }
}