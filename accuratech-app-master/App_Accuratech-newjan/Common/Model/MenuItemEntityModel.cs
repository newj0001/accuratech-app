using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using SQLite;
using SQLiteNetExtensions.Attributes;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Common
{
    [Table("MenuItemEntityModel")]
    public class MenuItemEntityModel : INotifyPropertyChanged
    {

        private int _id;
        [PrimaryKey]
        public int Id
        {
            get { return _id; }
            set { _id = value; NotifyPropertyChanged();  }
        }


        private string _header;

        public string Header
        {
            get { return _header; }
            set { _header = value; NotifyPropertyChanged(); }
        }

        private List<SubItemEntityModel> _subItems;
        [OneToMany(CascadeOperations = CascadeOperation.CascadeRead)]
        public List<SubItemEntityModel> SubItems
        {
            get { return _subItems; }
            set { _subItems = value; NotifyPropertyChanged(); }
        }


        [OneToMany(CascadeOperations = CascadeOperation.CascadeRead)]
        public List<RegistrationModel> Registrations { get; set; }

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
