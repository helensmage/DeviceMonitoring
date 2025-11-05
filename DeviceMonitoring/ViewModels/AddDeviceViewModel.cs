using DeviceMonitoring.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace DeviceMonitoring.ViewModels
{
    class AddDeviceViewModel : INotifyPropertyChanged
    {
        private string _category;
        private string _name;
        private string _serialNumber;
        private DateTime _installationDate;
        private DeviceStatus _status;

        public AddDeviceViewModel()
        {
            InstallationDate = DateTime.Today;

            OkCommand = new RelayCommand(_ => Ok(), _ => EmptyCheck());
            CancelCommand = new RelayCommand(_ => Cancel());
        }

        public string Category
        {
            get => _category;
            set
            {
                _category = value;
                OnPropertyChanged();
            }
        }

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        public string SerialNumber
        {
            get => _serialNumber;
            set
            {
                _serialNumber = value;
                OnPropertyChanged();
            }
        }

        public DateTime InstallationDate
        {
            get => _installationDate;
            set
            {
                _installationDate = value;
                OnPropertyChanged();
            }
        }

        public DeviceStatus Status
        {
            get => _status;
            set
            {
                _status = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand OkCommand { get; }
        public RelayCommand CancelCommand { get; }

        public bool? DialogResult { get; set; }

        public Action<bool?> CloseCallback { get; set; }

        public Array DeviceStatuses => Enum.GetValues(typeof(DeviceStatus));

        private void Ok()
        {
            if (EmptyCheck())
            {
                DialogResult = true;
                CloseCallback?.Invoke(true);
            }
        }

        private bool EmptyCheck()
        {
            return !string.IsNullOrWhiteSpace(Category) &&
                   !string.IsNullOrWhiteSpace(Name) &&
                   !string.IsNullOrWhiteSpace(SerialNumber);
        }

        private void Cancel()
        {
            DialogResult = false;
            CloseCallback?.Invoke(false);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
