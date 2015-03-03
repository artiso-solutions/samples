using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows.Controls;
using Contracts;

namespace Shell
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    [Export]
    public partial class MainWindow
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
