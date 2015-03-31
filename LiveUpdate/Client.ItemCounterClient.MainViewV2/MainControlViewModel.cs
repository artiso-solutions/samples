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

        private byte countSmall;

        private byte countBig;

        private string error;

        public MainControlViewModel()
        {
            ConnectToServiceCommand = new RelayCommand(ConnectToService);
            ResetCommand = new ThrowExceptionCommand();
            wcfClient = new WcfClient();
            wcfClient.DashboardUpdatedVersion("ClientA", "v2");
            wcfClient.OnCountChanged += CountChangedEventHandler;
            ConnectToService(null);
        }

        public RelayCommand ConnectToServiceCommand { get; private set; }

        public ThrowExceptionCommand ResetCommand { get; private set; }

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

        public int CountSmall
        {
            get
            {
                return countSmall;
            }
            set
            {
                if (countSmall == value)
                {
                    return;
                }

                countSmall = checked((byte)value);
                OnPropertyChanged();
            }
        }

        public int CountBig
        {
            get
            {
                return countBig;
            }
            set
            {
                if (countBig == value)
                {
                    return;
                }

                countBig = checked((byte)value);
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

            Error = string.Empty;
            wcfClient.Start(endpoint);
        }

        private void CountChangedEventHandler(object sender, CountChangedEventHandlerArgs args)
        {
            Count = args.CurrentCount;
            CountBig = args.CurrentBig;
            CountSmall = args.CurrentSmall;
        }
    }
}
