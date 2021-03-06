﻿using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.ServiceModel.Channels;
using Dashboard.Shell.Annotations;
using DashboardContracts;
using WpfBaseLibrary;

namespace Dashboard.Shell
{
    public class MainWindowViewModel : ViewModelBase, IMainWindowViewModel
    {
        private string _currentServiceVersion;
        private string _updateServiceVersion;
        private string _currentClientAVersion;
        private string _updateClientAVersion;
        private string _currentClientBVersion;
        private string _updateClientBVersion;
        private DashboardServiceHost _serviceHost;
        private ObservableCollection<DashboardMessage> _messages;

        public UpdateCommand UpdateCommand { get; set; }

        public MainWindowViewModel()
        {
            _serviceHost = new DashboardServiceHost(this);
            UpdateCommand = new UpdateCommand();
            Messages = new ObservableCollection<DashboardMessage>();
            //Messages.Add(new DashboardMessage() {Message = "Info", Timestamp=DateTime.Now, Type = MessageTypes.Info});
            //Messages.Add(new DashboardMessage() { Message = "Error", Timestamp = DateTime.Now, Type = MessageTypes.Error });
        }

        public ObservableCollection<DashboardMessage> Messages
        {
            get { return _messages; }
            set
            {
                if (value == _messages) return;
                _messages = value;
                OnPropertyChanged();
            }
        }

        public string CurrentServiceVersion
        {
            get { return _currentServiceVersion; }
            set
            {
                if (value == _currentServiceVersion) return;
                _currentServiceVersion = value;
                OnPropertyChanged();
            }
        }

        public string UpdateServiceVersion
        {
            get { return _updateServiceVersion; }
            set
            {
                if (value == _updateServiceVersion) return;
                _updateServiceVersion = value;
                OnPropertyChanged();
            }
        }

        public string CurrentClientAVersion
        {
            get { return _currentClientAVersion; }
            set
            {
                if (value == _currentClientAVersion) return;
                _currentClientAVersion = value;
                OnPropertyChanged();
            }
        }

        public string UpdateClientAVersion
        {
            get { return _updateClientAVersion; }
            set
            {
                if (value == _updateClientAVersion) return;
                _updateClientAVersion = value;
                OnPropertyChanged();
            }
        }

        public string CurrentClientBVersion
        {
            get { return _currentClientBVersion; }
            set
            {
                if (value == _currentClientBVersion) return;
                _currentClientBVersion = value;
                OnPropertyChanged();
            }
        }

        public string UpdateClientBVersion
        {
            get { return _updateClientBVersion; }
            set
            {
                if (value == _updateClientBVersion) return;
                _updateClientBVersion = value;
                OnPropertyChanged();
            }
        }
    }
}
