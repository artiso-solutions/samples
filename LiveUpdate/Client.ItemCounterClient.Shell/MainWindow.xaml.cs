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
using System.Windows.Shapes;

namespace Shell
{
   using System.ComponentModel.Composition;

   using ClientContracts;

   /// <summary>
   /// Interaction logic for MainWindow.xaml
   /// </summary>
   [Export]
   public partial class MainWindow : Window
   {
      private IEnumerable<Lazy<IMainControl, IMetaData>> contentControls;

      public MainWindow()
      {
         InitializeComponent();
      }

      [ImportMany(typeof(IMainControl), AllowRecomposition = true)]
      public IEnumerable<Lazy<IMainControl, IMetaData>> ContentControls
      {
         get
         {
            return this.contentControls;
         }
         set
         {
            this.contentControls = value;
            this.LoadLatestControl(value);
         }
      }

      private void LoadLatestControl(IEnumerable<Lazy<IMainControl, IMetaData>> controls)
      {
         var latestControl = controls.FirstOrDefault(c => c.Metadata.Version == controls.Max(a => a.Metadata.Version));
         if (latestControl != null)
         {
            if (this.Dispatcher.CheckAccess())
            {
               this.Content = latestControl.Value as Control;
            }
            else
            {
               this.Dispatcher.Invoke(() => { this.Content = latestControl.Value as Control; });
            }
         }
      }
   }
}
