using Common;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UI_Mobile.Models;

namespace UI_Mobile.ViewModels
{
    public class MainPageDetailViewModelOffline : INotifyPropertyChanged
    {
        public MainPageDetailViewModelOffline()
        {

        }

        public MainPageDetailViewModelOffline(SubItemEntityModel paramSubItem)
        {
            _subItemEntity = paramSubItem;
        }

        public void Reset(MenuItemEntityModel menuItemEntityOnline)
        {
            MenuItemEntityOnline = menuItemEntityOnline;
        }

        private MenuItemEntityModel _menuItemEntityOnline;
        public MenuItemEntityModel MenuItemEntityOnline
        {
            get => _menuItemEntityOnline;
            set
            {
                _menuItemEntityOnline = value;
                NotifyPropertyChanged();
            }
        }

        private SubItemEntityModel _subItemEntity;
        public SubItemEntityModel SubItemEntity
        {
            get => _subItemEntity;
            set
            {
                _subItemEntity = value;
                NotifyPropertyChanged();
            }
        }

        public string Header
        {
            get { return _menuItemEntityOnline.Header; }
            set
            {
                _menuItemEntityOnline.Header = value;
                NotifyPropertyChanged();
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
