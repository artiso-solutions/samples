using System.ComponentModel.Composition;
using System.Windows;

using Contracts;

namespace Logic
{
    [Export(typeof(IMainWindow))]
    [ExportMetadata("Version", 1)]
    public class MainWindow : Window, IMainWindow
    {
         
    }
}