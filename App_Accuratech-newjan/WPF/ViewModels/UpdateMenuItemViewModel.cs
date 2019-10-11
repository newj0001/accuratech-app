using Common;
using Common.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WPF.ViewModels
{
    public class UpdateMenuItemViewModel : INotifyPropertyChanged
    {

        MenuItemDataStore _menuItemDataStore = new MenuItemDataStore();
        FieldItemDataStore _fieldItemDataStore = new FieldItemDataStore();

        private ICollection<MenuItemEntityModel> _menuItemsCollection;
        private ICollection<SubItemEntityModel> _subItemsCollection;

        private readonly MenuItemEntityModel _parentMenuItem;

        public string MenuItemTitle { get; set; }
        public bool SelectedElementInIsMenuEnabled { get; set; }
        public UpdateMenuItemViewModel(MenuItemEntityModel menuItem)
        {
            _parentMenuItem = menuItem;
        }
        public async Task UpdateMenuItem()
        {
            var menuItem = new MenuItemEntityModel
            {
                Header = MenuItemTitle,
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

        public async Task Reset()
        {
            MenuItemsCollection = await _menuItemDataStore.GetItemsAsync();
            SubItemsCollection = await _fieldItemDataStore.GetItemsAsync();
        }

        public event EventHandler MenuItemUpdated;

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
