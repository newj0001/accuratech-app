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
    public class UpdateFieldItemViewModel : INotifyPropertyChanged
    {
        public enum FieldEnabled
        {
            Yes,
            No
        }

        public enum NumericField
        {
            Yes,
            No
        }
        public enum KeyboardInput
        {
            Enabled,
            Disabled
        }
        public enum EmptyField
        {
            Yes,
            No
        }
        public enum KeepFieldValue
        {
            Yes,
            No
        }

        private readonly FieldItemDataStore _fieldItemDataStore = new FieldItemDataStore();
        private readonly RegistrationDataStore _registrationItemDataStore = new RegistrationDataStore();
        private readonly MenuItemDataStore _menuItemDataStore = new MenuItemDataStore();
        private readonly SubItemEntityModel _parentSubItem;

        public UpdateFieldItemViewModel(SubItemEntityModel subItem)
        {
            _parentSubItem = subItem;
        }
        public string SubItemTitle
        {
            get { return _parentSubItem.Name; }
            set
            {
                _parentSubItem.Name = value;
                NotifyPropertyChanged();
            }
        }
        public int FieldMinLength { get; set; }
        public int FieldMaxLength { get; set; }
        public FieldEnabled SelectedElementInFieldEnabled { get; set; }
        public NumericField SelectedElementInNumericField { get; set; }
        public KeyboardInput SelectedElementInKeyboardInput { get; set; }
        public EmptyField SelectedElementInEmptyField { get; set; }
        public KeepFieldValue SelectedElementInKeepFieldValue { get; set; }

        private ICollection<RegistrationModel> _registrationCollection;
        public ICollection<RegistrationModel> RegistrationCollection
        {
            get => _registrationCollection;
            set
            {
                _registrationCollection = value;
                NotifyPropertyChanged();
            }
        }

        private ICollection<SubItemEntityModel> _subItemsCollection;
        public ICollection<SubItemEntityModel> SubItemsCollection
        {
            get => _subItemsCollection;
            set
            {
                _subItemsCollection = value;
                NotifyPropertyChanged();
            }
        }

        private ICollection<MenuItemEntityModel> _menuItemsCollection;
        public ICollection<MenuItemEntityModel> MenuItemsCollection
        {
            get => _menuItemsCollection;
            set
            {
                _menuItemsCollection = value;
                NotifyPropertyChanged();
            }
        }
        public async Task UpdateFieldItem()
        {
            var subItem = new SubItemEntityModel
            {
                Name = SubItemTitle,
                FieldValue = SubItemTitle,
                IsFieldEnabled = SelectedElementInFieldEnabled.ToString(),
                IsNumericFieldEnabled = SelectedElementInNumericField.ToString(),
                FieldMinLength = FieldMinLength,
                FieldMaxLength = FieldMaxLength,
                KeyboardInput = SelectedElementInKeyboardInput.ToString(),
                EmptyField = SelectedElementInEmptyField.ToString(),
                KeepFieldValue = SelectedElementInKeepFieldValue.ToString(),
                Id = _parentSubItem.Id,
                MenuItemId = _parentSubItem.MenuItemId
            };
            await _fieldItemDataStore.UpdateItemAsync(subItem, _parentSubItem.Id);
            SubItemUpdated?.Invoke(this, EventArgs.Empty);
        }

        public async Task Reset()
        {
            SubItemsCollection = await _fieldItemDataStore.GetItemsAsync();
            MenuItemsCollection = await _menuItemDataStore.GetItemsAsync();
            RegistrationCollection = await _registrationItemDataStore.GetItemsAsync();
        }

        public event EventHandler SubItemUpdated;

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
