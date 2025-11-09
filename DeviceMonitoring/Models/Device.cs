using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DeviceMonitoring.Models
{
    public class Device : INotifyPropertyChanged
    {
        private string _category;
        private string _name;
        private string _serialNumber;
        private DateTime _installationDate;
        private DeviceStatus _status;

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

        public Device Clone()
        {
            return new Device
            {
                Category = this.Category,
                Name = this.Name,
                SerialNumber = this.SerialNumber,
                InstallationDate = this.InstallationDate,
                Status = this.Status
            };
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
