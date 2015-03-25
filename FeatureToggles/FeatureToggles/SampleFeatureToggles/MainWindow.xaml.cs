namespace SampleFeatureToggles
{
   using System.Windows;

   using SampleFeatureToggles.FeatureToggles;

   /// <summary>
   /// Interaction logic for MainWindow.xaml
   /// </summary>
   public partial class MainWindow : Window
   {
      public MainWindow()
      {
         InitializeComponent();
      }

      private void Button_Click(object sender, RoutedEventArgs e)
      {
         if (new LoadCustomDataFeature().FeatureEnabled)
         {
            MessageBox.Show("Load Custom Data is enabled");
         }
      }
   }
}
