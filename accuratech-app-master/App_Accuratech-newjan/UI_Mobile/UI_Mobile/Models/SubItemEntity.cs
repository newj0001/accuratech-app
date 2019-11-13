using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace UI_Mobile.Models
{
    [Table("SubItems")]
    public class SubItemEntity
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Name { get; set; }


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

        [ForeignKey(typeof(MenuItemEntity))]
        public int MenuItemId { get; set; }

        private string _fieldValue;

        public string FieldValue
        {
            get { return _fieldValue; }
            set
            {
                _fieldValue = value;
            }
        }

        public string IsFieldEnabled { get; set; }
        public string IsNumericFieldEnabled { get; set; }
        public int NumericFieldEnabled
        {
            get
            {
                switch (IsNumericFieldEnabled)
                {
                    case "No":
                        return 0;
                    case "Yes":
                        return 1;

                    default: return 0;
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
        public int ScanEnabled
        {
            get
            {
                switch (IsScanEnabled)
                {
                    case "Disabled":
                        return 0;

                    case "Enabled":
                        return 1;

                    default: return 0;
                }
            }
            private set { }
        }

        public string Type { get; set; }
        public int Length { get; set; }
        public string StartWith { get; set; }
        public int Offset { get; set; }
        public int ValueLength { get; set; }
    }
}
