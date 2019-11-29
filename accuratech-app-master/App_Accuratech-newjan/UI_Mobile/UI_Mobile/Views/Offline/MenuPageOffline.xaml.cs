using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UI_Mobile.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace UI_Mobile.Views.Offline
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuPageOffline : ContentPage
    {
        MainPageOffline RootPage { get => Application.Current.MainPage as MainPageOffline; }
        List<HomeMenuItem> menuItems;
        public MenuPageOffline()
        {
            InitializeComponent();
            menuItems = new List<HomeMenuItem>
            {
                new HomeMenuItem {Id = MenuItemType.Browse, Title="Browse" }
            };


            ListViewMenu.ItemsSource = menuItems;

            ListViewMenu.SelectedItem = menuItems[0];
            ListViewMenu.ItemSelected += async (sender, e) =>
            {
                if (e.SelectedItem == null)
                    return;

                var id = (int)((HomeMenuItem)e.SelectedItem).Id;
                await RootPage.NavigateFromMenu(id);
            };
        }
    }
}