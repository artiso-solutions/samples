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

      private string endpoint;

      private string error;
      public MainControlViewModel()
      {
         ConnectToServiceCommand = new RelayCommand(ConnectToService);
         wcfClient = new WcfClient();
         wcfClient.OnCountChanged += CountChangedEventHandler;
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
            endpoint = wcfClient.GetEndpointFromDispatcher();
         }
         catch (Exception)
         {
            Error = "Error: Service not started";
            return;
         }

         if (string.IsNullOrEmpty(endpoint))
         {
            Error = "Error: Endpoint to service not found";
            return;
         }

         wcfClient.Start(endpoint);
      }

      private void CountChangedEventHandler(object sender, CountChangedEventHandlerArgs args)
      {
         Count = args.CurrentCount;
      }
   }
}
