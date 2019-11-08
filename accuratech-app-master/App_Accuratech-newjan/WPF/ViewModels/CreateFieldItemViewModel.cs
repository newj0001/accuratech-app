using Common;
using Common.Services;
using Common.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WPF.ViewModels
{
    #region Enums
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

    public enum ScanEnabled
    {
        Enabled,
        Disabled
    }

    public enum ScanType
    {
        Fixed,
        GSI
    }
    #endregion Enums
    public class CreateFieldItemViewModel
    {
        private readonly FieldItemDataStore _fieldItemDataStore = new FieldItemDataStore();
        private readonly MenuItemEntityModel _parentMenuItem;

        public string SubItemTitle { get; set; }
        public int FieldMinLength { get; set; }
        public int FieldMaxLength { get; set; }
        public int Length { get; set; }
        public string StartWith { get; set; }
        public int Offset { get; set; }
        public int ValueLength { get; set; }
        public FieldEnabled SelectedElementInFieldEnabled { get; set; }
        public NumericField SelectedElementInNumericField { get; set; }
        public KeyboardInput SelectedElementInKeyboardInput { get; set; }
        public EmptyField SelectedElementInEmptyField { get; set; }
        public KeepFieldValue SelectedElementInKeepFieldValue { get; set; }
        public ScanEnabled SelectedElementInScanEnabled { get; set; }
        public ScanType SelectedElementInScanType { get; set; }
        public CreateFieldItemViewModel(MenuItemEntityModel menuItem)
        {
            _parentMenuItem = menuItem;
        }

        public async Task AddFieldItem()
        {
           
            var subItem = new SubItemEntityModel()
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
                MenuItemId = _parentMenuItem.Id,
                IsScanEnabled = SelectedElementInScanEnabled.ToString(),
                Type = SelectedElementInScanType.ToString(),
                Length = Length,
                StartWith = StartWith,
                Offset = Offset,
                ValueLength = ValueLength
            };

            await _fieldItemDataStore.AddItemAsync(subItem);
            NewSubItemCreated?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler NewSubItemCreated;
    }
}
