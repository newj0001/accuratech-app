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
    public class RegistrationListViewModel : INotifyPropertyChanged
    {
        private readonly RegistrationDataStore _registrationDataStore = new RegistrationDataStore();

        public RegistrationListViewModel()
        {

        }

        private MenuItemEntityModel _menuItemEntity;

        public MenuItemEntityModel MenuItemEntity
        {
            get { return _menuItemEntity; }
            set 
            {
                _menuItemEntity = value;
                NotifyPropertyChanged();
            }
        }


        private ICollection<RegistrationModel> _registrationCollection;

        public ICollection<RegistrationModel> RegistrationCollection
        {
            get { return _registrationCollection; }
            set 
            {
                _registrationCollection = value; 
                NotifyPropertyChanged();  
            }
        }



        public async Task Reset()
        {
            RegistrationCollection = await _registrationDataStore.GetItemsAsync();
        }


        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
