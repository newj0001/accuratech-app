using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace UI_Mobile.Models
{
    [Table("RegistrationValueItemEntity")]
    public class RegistrationValueItemEntity
    {

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int SubItemId { get; set; }
        public string SubItemName { get; set; }

        private string _value;
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
