using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Common
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
