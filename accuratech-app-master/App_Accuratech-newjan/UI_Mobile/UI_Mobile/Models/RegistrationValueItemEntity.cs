using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace UI_Mobile.Models
{
    [Table("RegistrationValueItems")]
    public class RegistrationValueItemEntity
    {
        private string _value;

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int SubItemId { get; set; }
        public string SubItemName { get; set; }

        [ManyToOne]
        public SubItemEntity SubItemEntity { get; set; }

        public string Value
        {
            get => _value;
            set
            {
                _value = value;
            }
        }

        [ForeignKey(typeof(RegistrationItemEntity))]
        public int RegistrationId { get; set; }
    }
}
