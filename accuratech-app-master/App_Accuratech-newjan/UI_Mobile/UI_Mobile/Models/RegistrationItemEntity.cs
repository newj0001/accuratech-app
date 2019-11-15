using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace UI_Mobile.Models
{
    [Table("RegistrationItemEntity")]
    public class RegistrationItemEntity
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.Now;

        [ForeignKey(typeof(MenuItemEntity))]
        public int MenuItemId { get; set; }

        [ManyToOne(CascadeOperations = CascadeOperation.CascadeRead)]
        public MenuItemEntity MenuItemEntity { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.CascadeRead)]
        public List<RegistrationValueItemEntity> RegistrationValues { get; set; } = new List<RegistrationValueItemEntity>();
    }
}
