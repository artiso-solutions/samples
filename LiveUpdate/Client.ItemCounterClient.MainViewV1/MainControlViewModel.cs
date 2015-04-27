using System.IO;
using System.Reflection;
using System.Security.Policy;
using ClientContracts;

namespace ItemCounterClient.MainView
{
   using System;
   using System.Windows;

   using CommunicationClient;

   using WpfBaseLibrary;

   public class MainControlViewModel : ViewModelBase
   {
      private int count;

      private readonly WcfClient wcfClient;

      private string dispatcherEndpoint;

      private string error;

       public MainControlViewModel()
       {
           ConnectToServiceCommand = new RelayCommand(ConnectToService);
           wcfClient = new WcfClient();
           wcfClient.OnCountChanged += CountChangedEventHandler;
           ConnectToService(null);
       }

       public RelayCommand ConnectToServiceCommand { get; private set; }

      public int Count
      {
         get
         {
            return count;
         }
         set
         {
            if (count == value)
            {
               return;
            }

            count = value;
            OnPropertyChanged();
         }
      }

      public string Error
      {
         get
         {
            return error;
         }
         set
         {
            if (error == value)
            {
               return;
            }

            error = value;
            OnPropertyChanged();
         }
      }

      private void ConnectToService(object arg)
      {
         try
         {
            dispatcherEndpoint = wcfClient.GetEndpointFromDispatcher();
         }
         catch (Exception)
         {
            Error = "Error: Service not started";
            return;
         }

         if (string.IsNullOrEmpty(dispatcherEndpoint))
         {
            Error = "Error: Endpoint to service not found";
            return;
         }

         wcfClient.Start(dispatcherEndpoint);
      }

      private void CountChangedEventHandler(object sender, CountChangedEventHandlerArgs args)
      {
         Count = args.CurrentCount;
      }
   }
}
