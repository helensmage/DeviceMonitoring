using DeviceMonitoring.Models;
using DeviceMonitoring.Views;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Data;
using System.Windows.Input;
using System.Xml.Linq;

namespace DeviceMonitoring.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly ObservableCollection<Device> _devices;
        private Device _selectedDevice;
        private Device _editingDevice;
        private string _searchText;
        private DeviceStatus? _statusFilter;

        public MainViewModel()
        {
            _devices = new ObservableCollection<Device>();
            DevicesView = CollectionViewSource.GetDefaultView(_devices);
            DevicesView.GroupDescriptions.Add(new PropertyGroupDescription("Category"));
            DevicesView.Filter = FilterDevices;

            AddDeviceCommand = new RelayCommand(_ => OpenAddDeviceDialog());
            DeleteDeviceCommand = new RelayCommand(_ => DeleteDeviceDialog(), _ => SelectedDevice != null);
            SaveCommand = new RelayCommand(_ => SaveChanges(), _ => EmptyCheck());
            CancelCommand = new RelayCommand(_ => CancelChanges(), _ => SelectedDevice != null);
        }

        public ICollectionView DevicesView { get; }

        public Device SelectedDevice
        {
            get => _selectedDevice;
            set
            {
                _selectedDevice = value;
                if (value != null)
                {
                    EditingDevice = value.Clone();
                }
                OnPropertyChanged();
            }
        }

        public Device EditingDevice
        {
            get => _editingDevice;
            set
            {
                _editingDevice = value;
                OnPropertyChanged();
            }
        }

        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged();
                DevicesView.Refresh();
            }
        }

        public DeviceStatus? StatusFilter
        {
            get => _statusFilter;
            set
            {
                _statusFilter = value;
                OnPropertyChanged();
                DevicesView.Refresh();
            }
        }

        public RelayCommand AddDeviceCommand { get; }
        public RelayCommand DeleteDeviceCommand { get; }
        public RelayCommand SaveCommand { get; }
        public RelayCommand CancelCommand { get; }

        public Array DeviceStatuses => Enum.GetValues(typeof(DeviceStatus));

        private bool FilterDevices(object obj)
        {
            if (obj is not Device device) return false;

            bool searchMatch = string.IsNullOrEmpty(SearchText) ||
                             device.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase);

            bool statusMatch = !StatusFilter.HasValue || device.Status == StatusFilter.Value;

            return searchMatch && statusMatch;
        }

        private void OpenAddDeviceDialog()
        {
            var addDeviceViewModel = new AddDeviceViewModel();
            var dialog = new AddDeviceDialog { DataContext = addDeviceViewModel };

            addDeviceViewModel.CloseCallback = (result) => dialog.DialogResult = result;

            if (dialog.ShowDialog() == true)
            {
                var newDevice = new Device
                {
                    Category = addDeviceViewModel.Category,
                    Name = addDeviceViewModel.Name,
                    SerialNumber = addDeviceViewModel.SerialNumber,
                    InstallationDate = addDeviceViewModel.InstallationDate,
                    Status = addDeviceViewModel.Status
                };
                _devices.Add(newDevice);
            }
        }

        private void DeleteDeviceDialog()
        {
            if (SelectedDevice != null)
            {
                var result = System.Windows.MessageBox.Show(
                    $"Вы уверены, что хотите удалить устройство '{SelectedDevice.Name}'?",
                    "Подтверждение удаления",
                    System.Windows.MessageBoxButton.YesNo,
                    System.Windows.MessageBoxImage.Question);

                if (result == System.Windows.MessageBoxResult.Yes)
                {
                    _devices.Remove(SelectedDevice);
                    SelectedDevice = null;
                }
            }
        }

        private bool EmptyCheck()
        {
            if (EditingDevice != null && SelectedDevice != null)
            {
                return !string.IsNullOrWhiteSpace(EditingDevice.Category) &&
                   !string.IsNullOrWhiteSpace(EditingDevice.Name) &&
                   !string.IsNullOrWhiteSpace(EditingDevice.SerialNumber);
            }
            else
            {
                return false;
            }
        }

        private void SaveChanges()
        {
            if (EditingDevice != null && SelectedDevice != null)
            {
                SelectedDevice.Category = EditingDevice.Category;
                SelectedDevice.Name = EditingDevice.Name;
                SelectedDevice.SerialNumber = EditingDevice.SerialNumber;
                SelectedDevice.InstallationDate = EditingDevice.InstallationDate;
                SelectedDevice.Status = EditingDevice.Status;

                DevicesView.Refresh();
            }
        }

        private void CancelChanges()
        {
            if (SelectedDevice != null)
            {
                EditingDevice = SelectedDevice.Clone();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
