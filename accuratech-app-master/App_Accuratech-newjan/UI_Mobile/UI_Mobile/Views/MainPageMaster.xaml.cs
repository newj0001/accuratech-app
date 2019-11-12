using Common;
using System;
using System.Diagnostics;
using UI_Mobile.Models;
using UI_Mobile.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace UI_Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPageMaster : ContentPage
    {
        private readonly MenuItemDataStore _menuItemDataStore = new MenuItemDataStore();
        private MenuItemEntityModel _parentMenuItem;
        MainPageMasterViewModel mainPageMasterViewModel = new MainPageMasterViewModel();
        public MainPageMaster()
        {
            InitializeComponent();
            BindingContext = mainPageMasterViewModel;
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
            await mainPageMasterViewModel.Reset();
        }

        protected override void OnDisappearing()
        {
            Connectivity.ConnectivityChanged -= Connectivity_ConnectivityChanged;
        }

        private async void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            if (e.NetworkAccess == NetworkAccess.None)
            {
                
                var previousMenuItem = new MenuItemEntity
                {
                    Header = "Test1",

                    
                };
                await App.MenuItemDatabase.SaveMenuItemAsync(previousMenuItem);
                MenuListView.ItemsSource = await App.MenuItemDatabase.GetMenuItemsAsync();
                await LabelConnection.FadeTo(1).ContinueWith((result) => { });
            }
            else
            {
                await LabelConnection.FadeTo(0).ContinueWith((result) => { });
                await mainPageMasterViewModel.Reset();
                MenuListView.ItemsSource = await _menuItemDataStore.GetItemsAsync();
            }
        }

        private async void OnItemSelected(object sender, ItemTappedEventArgs e)
        {
            var selectedItem = e.Item as MenuItemEntityModel;

            if (!selectedItem.IsMenuEnabledAsBool)
            {
                return;
            }

            await Navigation.PushAsync(new MainPageDetail(selectedItem));
            ((ListView)sender).SelectedItem = null;
        }
    }
}