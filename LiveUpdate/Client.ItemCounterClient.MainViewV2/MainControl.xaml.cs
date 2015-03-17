using System.ComponentModel.Composition;
using ClientContracts;
using System.Windows;

namespace ItemCounterClient.MainView
{
   using System;

   using CommunicationClient;

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
