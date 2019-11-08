using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;


namespace Common.Standard.Entities
{
    public class SubItemEntityModel : INotifyPropertyChanged
    {
        public int Id { get; set; }

        private string _name;

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                NotifyPropertyChanged();
            }
        }

        public bool IsFieldEnabledAsBool
        {
            get
            {
                switch (IsFieldEnabled)
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

        public int? MenuItemId { get; set; }

        private string _fieldValue;

        public string FieldValue
        {
            get { return _fieldValue; }
            set 
            {
                _fieldValue = value; 
                NotifyPropertyChanged(); 
            }
        }

        public string IsFieldEnabled { get; set; }
        public string IsNumericFieldEnabled { get; set; }
        public bool NumericFieldEnabled
        {
            get
            {
                switch (IsNumericFieldEnabled)
                {
                    case "No":
                        return false;
                    case "Yes":
                        return true;

                    default: return false;
                }
            }
            private set { }
        }
        public int FieldMinLength { get; set; }
        public int FieldMaxLength { get; set; }
        public string KeyboardInput { get; set; }
        public string EmptyField { get; set; }
        public string KeepFieldValue { get; set; }
        public string IsScanEnabled { get; set; }
        public bool ScanEnabled
        {
            get
            {
                switch (IsScanEnabled)
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

        public string Type { get; set; }
        public int Length { get; set; }
        public string StartWith { get; set; }
        public int Offset { get; set; }
        public int ValueLength { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
