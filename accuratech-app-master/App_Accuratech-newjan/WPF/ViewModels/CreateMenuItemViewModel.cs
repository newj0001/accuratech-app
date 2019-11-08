using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF.ViewModels
{
    public enum IsMenuEnabled
    {
        Enabled,
        Disabled
    }
    public class CreateMenuItemViewModel
    {
        private readonly MenuItemDataStore _menuItemDataStore = new MenuItemDataStore();
        public string MenuItemTitle { get; set; }
        public IsMenuEnabled SelectedElementInIsMenuEnabled { get; set; }
        public async Task AddMenuItem()
        {
            var menuItem = new MenuItemEntityModel
            {
                Header = MenuItemTitle,
                IsMenuEnabled = SelectedElementInIsMenuEnabled.ToString()
            };
            await _menuItemDataStore.AddItemAsync(menuItem);
            NewMenuItemCreated?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler NewMenuItemCreated;
    }
}
