using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class RegistrationValueModel : INotifyPropertyChanged
    {
        private string _value;
        public int Id { get; set; }
        public int SubItemId { get; set; }
        public string SubItemName { get; set; }
        public SubItemEntityModel SubItemEntityModel { get; set; }

        public string Value
        {
            get => _value;
            set
            {
                _value = value;
                NotifyPropertyChanged();
            }
        }

        public int RegistrationId { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
