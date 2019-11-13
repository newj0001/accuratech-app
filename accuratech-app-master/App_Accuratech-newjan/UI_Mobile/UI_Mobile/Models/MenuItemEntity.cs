using Common;
using Newtonsoft.Json;
using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace UI_Mobile.Models
{
    [Table("MenuItems")]
    public class MenuItemEntity : INotifyPropertyChanged
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Header { get; set; }


        private ICollection<SubItemEntity> _subItems;
        
        [OneToMany]
        public ICollection<SubItemEntity> SubItems 
        { 
            get => _subItems;
            set
            {
                _subItems = value;
                NotifyPropertyChanged();
            }
        }

        [OneToMany]
        public ICollection<RegistrationItemEntity> Registrations { get; set; }

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
