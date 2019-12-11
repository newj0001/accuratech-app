using System;
using System.Collections.Generic;
using SQLite;
using SQLiteNetExtensions.Attributes;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [Table("RegistrationValueModel")]
    public class RegistrationValueModel
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

        [ForeignKey(typeof(RegistrationModel))]
        public int RegistrationId { get; set; }

    }
}
