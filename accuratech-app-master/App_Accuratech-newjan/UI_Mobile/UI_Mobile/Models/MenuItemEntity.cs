using Common;
using Newtonsoft.Json;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace UI_Mobile.Models
{
    public class MenuItemEntity
    {
        private int _id;

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

        //private string _subItems;
        ////private ICollection<SubItemEntityModel> _subItems;

        //[Ignore]
        //public ICollection<SubItemEntityModel> SubItems
        //{
        //    get
        //    {
        //        return JsonConvert.DeserializeObject<ICollection<SubItemEntityModel>>(_subItems);
        //    }
        //    set
        //    {
        //        _subItems = JsonConvert.SerializeObject(value);
        //    }
        //}

        //public ICollection<RegistrationModel> Registrations { get; set; }

        //public bool IsMenuEnabledAsBool
        //{
        //    get
        //    {
        //        switch (IsMenuEnabled)
        //        {
        //            case "Disabled":
        //                return false;

        //            case "Enabled":
        //                return true;

        //            default: return false;
        //        }
        //    }
        //    private set { }
        //}

        //public string IsMenuEnabled { get; set; }


    }
}
