using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using UI_Mobile.Models;

namespace UI_Mobile.ViewModels
{
    public class MainPageDetailViewModelOffline : INotifyPropertyChanged
    {
        public MainPageDetailViewModelOffline()
        {

        }
        public MainPageDetailViewModelOffline(SubItemEntity paramSubItem)
        {
            _subItemEntity = paramSubItem;
        }

        public void Reset(MenuItemEntity menuItemEntity)
        {
            MenuItemEntity = menuItemEntity;
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
        public string Header
        {
            get { return _menuItemEntity.Header; }
            set
            {
                _menuItemEntity.Header = value;
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
