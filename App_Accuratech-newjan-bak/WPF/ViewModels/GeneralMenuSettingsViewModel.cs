using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WPF.ViewModels
{
    public class GeneralMenuSettingsViewModel : INotifyPropertyChanged
    {
        MenuItemDataStore _menuItemDataStore = new MenuItemDataStore();
        FieldItemDataStore _fieldItemDataStore = new FieldItemDataStore();
        RegistrationDataStore _registrationItemDataStore = new RegistrationDataStore();
        private readonly MenuItemEntityModel _parentMenuItem;
        private ICollection<MenuItemEntityModel> _menuItemsCollection;
        private ICollection<SubItemEntityModel> _subItemsCollection;
        private ICollection<RegistrationModel> _registrationCollection;
        public GeneralMenuSettingsViewModel(MenuItemEntityModel menuItem)
        {
            _parentMenuItem = menuItem;
        }

        public string Header
        {
            get { return _parentMenuItem.Header; }
            set { _parentMenuItem.Header = value; NotifyPropertyChanged(); }
        }


        public bool SelectedElementInIsMenuEnabled { get; set; }

        public async Task Update()
        {
            var menuItem = new MenuItemEntityModel
            {
                Header = _parentMenuItem.Header,
                IsMenuEnabled = SelectedElementInIsMenuEnabled.ToString(),
                Id = _parentMenuItem.Id
            };
            await _menuItemDataStore.UpdateItemAsync(menuItem, _parentMenuItem.Id);
            MenuItemUpdated?.Invoke(this, EventArgs.Empty);
        }

        public ICollection<MenuItemEntityModel> MenuItemsCollection
        {
            get => _menuItemsCollection;
            set
            {
                _menuItemsCollection = value;
                NotifyPropertyChanged();
            }
        }

        public ICollection<SubItemEntityModel> SubItemsCollection
        {
            get => _subItemsCollection;
            set
            {
                _subItemsCollection = value;
                NotifyPropertyChanged();
            }
        }

        public ICollection<RegistrationModel> RegistrationCollection
        {
            get => _registrationCollection;
            set
            {
                _registrationCollection = value;
                NotifyPropertyChanged();
            }
        }
        public async Task Reset()
        {
            MenuItemsCollection = await _menuItemDataStore.GetItemsAsync();
            SubItemsCollection = await _fieldItemDataStore.GetItemsAsync();
            RegistrationCollection = await _registrationItemDataStore.GetItemsAsync(_parentMenuItem.Id);
        }

        public event EventHandler MenuItemUpdated;

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
