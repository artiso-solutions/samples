using System.ComponentModel.Composition;
using ClientContracts;
using System.Windows;

namespace ItemCounterClient.MainView
{
    using System;
    using System.Collections.Generic;
    using CommunicationClient;

    /// <summary>
    /// Interaction logic for MainControl.xaml
    /// </summary>
    [Export(typeof(IMainControl))]
    [ExportMetadata("Version", 1)]
    public partial class MainControl : IMainControl
    {
        public MainControl()
        {
            InitializeComponent();
        }
    }
}
