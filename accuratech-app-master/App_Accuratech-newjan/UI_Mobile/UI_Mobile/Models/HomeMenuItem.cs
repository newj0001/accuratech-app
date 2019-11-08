using System;
using System.Collections.Generic;
using System.Text;

namespace UI_Mobile.Models
{
    public enum MenuItemType
    {
        Browse,
        Exit
    }
    public class HomeMenuItem
    {
        public MenuItemType Id { get; set; }

        public string Title { get; set; }
    }
}
