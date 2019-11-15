using Common;
using Newtonsoft.Json;
using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace UI_Mobile.Models
{
    [Table("MenuItemEntity")]
    public class MenuItemEntity : INotifyPropertyChanged
    {
        [PrimaryKey]
        public int Id { get; set; }

        public string Header { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.CascadeRead)]
        public List<SubItemEntity> SubItems { get; set; }


        //[OneToMany(CascadeOperations = CascadeOperation.CascadeRead)]
        //public List<RegistrationItemEntity> Registrations { get; set; }

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
            set { }
        }

        public string IsMenuEnabled { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
