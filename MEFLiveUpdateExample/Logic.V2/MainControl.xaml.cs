using System.ComponentModel.Composition;
using Contracts;

namespace Logic
{
   using System.Windows;

   /// <summary>
    /// Interaction logic for MainControl.xaml
    /// </summary>
    [Export(typeof(IMainControl))]
    [ExportMetadata("Version", 2)]
    public partial class MainControl : IMainControl
    {
        public MainControl()
        {
            InitializeComponent();
        }
    }
}
