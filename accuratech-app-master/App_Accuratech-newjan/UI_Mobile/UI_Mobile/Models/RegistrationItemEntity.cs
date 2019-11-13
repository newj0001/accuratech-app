using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace UI_Mobile.Models
{
    [Table("RegistrationItems")]
    public class RegistrationItemEntity
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.Now;

        [ForeignKey(typeof(MenuItemEntity))]
        public int MenuItemId { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public ICollection<RegistrationValueItemEntity> RegistrationValues { get; set; } = new List<RegistrationValueItemEntity>();
    }
}
