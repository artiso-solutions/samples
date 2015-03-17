using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ItemCounterClient
{
   /// <summary>
   /// Interaction logic for MainWindow.xaml
   /// </summary>
   public partial class MainWindow : Window
   {
      private readonly WcfClient wcfClient;

      private string endpoint;

      public MainWindow()
      {
         InitializeComponent();
         wcfClient = new WcfClient(this);
      }

      

      private void StartStopButton_Click(object sender, RoutedEventArgs e)
      {

         if (StartStopButton.Content.ToString() == "Start")
         {
            wcfClient.StartEngine();
         }
         else
         {
            wcfClient.StopEngine();
         }
      }

        private void ConnectButton_Click(object sender, RoutedEventArgs e)
        {
            endpoint = wcfClient.GetEndpointFromDispatcher();
            wcfClient.SubsrcibeToEndPoint(endpoint);
        }
    }
}
