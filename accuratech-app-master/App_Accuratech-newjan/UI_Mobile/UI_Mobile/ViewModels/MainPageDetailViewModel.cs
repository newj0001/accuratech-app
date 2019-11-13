using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Common;
using Common.Services;
using Common.ViewModel;
using UI_Mobile.Models;
using Xamarin.Forms;

namespace UI_Mobile.ViewModels
{
    public class MainPageDetailViewModel : INotifyPropertyChanged
    {
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

        private MenuItemEntityModel _menuItemEntityModel;
        public MenuItemEntityModel MenuItemEntityModel
        {
            get => _menuItemEntityModel;
            set
            {
                _menuItemEntityModel = value;
                NotifyPropertyChanged();
            }
        }

        private MenuItemEntity _menuItemEntity;
        public MenuItemEntity MenuItemEntity
        {
            get => _menuItemEntity;
            set
            {
                _menuItemEntity = value;
                NotifyPropertyChanged();
            }
        }
        private SubItemEntityModel _subItemEntityModel;
        public SubItemEntityModel SubItemEntityModel
        {
            get => _subItemEntityModel;
            set
            {
                _subItemEntityModel = value;
                NotifyPropertyChanged();
            }
        }

        private SubItemEntity _subItemEntity;
        public SubItemEntity SubItemEntity
        {
            get => _subItemEntity;
            set
            {
                _subItemEntity = value;
                NotifyPropertyChanged();
            }
        }

        public MainPageDetailViewModel() { }
        public MainPageDetailViewModel(SubItemEntityModel paramSubItem)
        {
            _subItemEntityModel = paramSubItem;
        }

        public MainPageDetailViewModel(SubItemEntity paramSubItem)
        {
            _subItemEntity = paramSubItem;
        }

        //public string Header
        //{
        //    get { return _menuItemEntityModel.Header; }
        //    set
        //    {
        //        _menuItemEntityModel.Header = value;
        //        NotifyPropertyChanged();
        //    }
        //}

        public string Header
        {
            get { return _menuItemEntity.Header; }
            set
            {
                _menuItemEntity.Header = value;
                NotifyPropertyChanged();
            }
        }

        //private static bool ValidateNumericField(SubItemEntityModel subItemEntity)
        //{
        //    switch (subItemEntity.NumericField.ToUpper())
        //    {
        //        case "YES":
        //            var res = int.TryParse(subItemEntity.FieldValue, out var _);
        //            return res;

        //        default:
        //            return true;
        //    }
        //}

        //private static bool ValidateLength(SubItemEntityModel subItemEntity)
        //{
        //    int minlen = subItemEntity.FieldMinLength;
        //    int maxlen = subItemEntity.FieldMaxLength;

        //    var minValid = subItemEntity.FieldMinLength <= minlen;
        //    var maxValid = true;

        //    if (maxlen > 0)
        //    {
        //        maxValid = subItemEntity.FieldMaxLength >= maxlen;
        //    }

        //    return minValid && maxValid;
        //}

        //public async Task AddRegistrationValue(SubItemEntityModel subItemEntity)
        //{
        //    if (subItemEntity == null) return;
        //    var fieldValue = _subItemEntityModel.FieldValue;


        //    ICollection<RegistrationValueModel> registrationValues = new List<RegistrationValueModel>();
        //    var subItem = new RegistrationValueModel()
        //    {
        //        SubItemId = _subItemEntityModel.Id,
        //        SubItemEntityModel = _subItemEntityModel,
        //        Value = fieldValue,
        //        SubItemName = _subItemEntityModel.Name
        //    };

        //    //await _datastore.AddItemAsync(registrationValues);
        //    NewRegistrationValueCreated?.Invoke(this, EventArgs.Empty);
        //}

        public void Reset(MenuItemEntityModel menuItemEntityModel)
        {
            MenuItemEntityModel = menuItemEntityModel;
        }

        public void Reset(MenuItemEntity menuItemEntity)
        {
            MenuItemEntity = menuItemEntity;
        }

        public event EventHandler NewRegistrationValueCreated;

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
