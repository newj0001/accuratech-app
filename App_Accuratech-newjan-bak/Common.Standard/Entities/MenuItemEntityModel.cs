using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Common.Standard.Entities
{
    public class MenuItemEntityModel : INotifyPropertyChanged
    {
        private int _id;

        public int Id
        {
            get => _id;
            set
            {
                _id = value;
                NotifyPropertyChanged();
            }
        }

        private string _header;

        public string Header
        {
            get { return _header; }
            set 
            { 
                _header = value; 
                NotifyPropertyChanged();
            }
        }


        private ICollection<SubItemEntityModel> _subItems;

        public ICollection<SubItemEntityModel> SubItems
        {
            get => _subItems;
            set
            {
                _subItems = value;
                NotifyPropertyChanged();
            }
        }

        public ICollection<RegistrationModel> Registrations { get; set; }

        public bool IsMenuEnabledAsBool
        {
            get
            {
                switch (IsMenuEnabled)
                {
                    case "Disabled":
                        return false;
                        
                    case "Enabled":
                        return true;
                        
                    default: return false;
                }
            }
            private set { }
        }

        public string IsMenuEnabled { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
