using System;
using System.Collections.Generic;
using SQLite;
using SQLiteNetExtensions.Attributes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [Table("RegistrationModel")]
    public class RegistrationModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.Now;

        [ForeignKey(typeof(MenuItemEntityModel))]
        public int MenuItemId { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.CascadeRead)]
        public List<RegistrationValueModel> RegistrationValues { get; set; }
    }
}
