using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Common.ViewModel
{
    public class NewItemViewModel : INotifyPropertyChanged
    {
        private readonly SubItemEntityModel _parentSubItem;
        private SubItemEntityModel _subItemEntityModel;

        public NewItemViewModel(SubItemEntityModel subItem)
        {
            _parentSubItem = subItem;
        }

        public SubItemEntityModel SubItemEntityModel
        {
            get => _subItemEntityModel;
            set
            {
                _subItemEntityModel = value;
                NotifyPropertyChanged();
            }
        }

        public string FieldValue { get; set; }
        public async Task AddRegistrationValue(string fieldValue)
        {
            ICollection<RegistrationValueModel> registrationValues = new List<RegistrationValueModel>();
            var regItem = new RegistrationValueModel()
            {
                SubItemId = _parentSubItem.Id,
                SubItemEntityModel = _parentSubItem,
                Value = fieldValue
            };
            registrationValues.Add(regItem);
            await Processor.CreateRegistrationValue(registrationValues);
            NewRegistrationValueCreated?.Invoke(this, EventArgs.Empty);
        }

        public void Reset(SubItemEntityModel subItemEntityModel)
        {
            SubItemEntityModel = subItemEntityModel;
        }

        public event EventHandler NewRegistrationValueCreated;
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
