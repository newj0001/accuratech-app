using Common;
using Newtonsoft.Json;
using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace UI_Mobile.Models
{
    [Table("MenuItems")]
    public class MenuItemEntity
    {
        private int _id;
        [PrimaryKey, AutoIncrement]
        public int Id
        {
            get => _id;
            set
            {
                _id = value;
            }
        }

        private string _header;

        public string Header
        {
            get { return _header; }
            set
            {
                _header = value;
            }
        }


        private ICollection<SubItemEntityModel> _subItems;

        [TextBlob("SubItemsBlobbed")]
        public ICollection<SubItemEntityModel> SubItems
        {
            get => _subItems;
            set
            {
                _subItems = value;
            }
        }
        public string SubItemsBlobbed { get; set; }
    }
}
