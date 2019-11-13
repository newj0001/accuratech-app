using Common;
using System;
using System.Collections.Generic;
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
                MenuListView.ItemsSource = await App.MenuItemDatabase.GetMenuItemsAsync();
                await LabelConnection.FadeTo(1).ContinueWith((result) => { });
            }
            else
            {
                await LabelConnection.FadeTo(0).ContinueWith((result) => { });
                var items = await _menuItemDataStore.GetItemsAsync();
                await App.MenuItemDatabase.DeleteAllMenuItemAsync();
                await App.MenuItemDatabase.SaveMenuItemsAsync(ConvertToEntity(items));
                MenuListView.ItemsSource = items;
            }
        }

        private ICollection<MenuItemEntity> ConvertToEntity(ICollection<MenuItemEntityModel> items)
        {
            var entities = new List<MenuItemEntity>();
            foreach (var item in items)
            {
                var x = new MenuItemEntity();
                x.Header = item.Header;
                x.IsMenuEnabled = item.IsMenuEnabled;
                x.SubItems = ConvertToSubEntity(item.SubItems);

                entities.Add(x);
            }

            return entities;
        }

        private ICollection<SubItemEntity> ConvertToSubEntity(ICollection<SubItemEntityModel> subItems)
        {
            var entities = new List<SubItemEntity>();
            foreach (var item in subItems)
            {
                var x = new SubItemEntity();
                {
                    x.FieldValue = item.FieldValue;

                    entities.Add(x);
                }
            }
            return entities;
        }

        private async void OnItemSelected(object sender, ItemTappedEventArgs e)
        {
            var selectedItem = e.Item as MenuItemEntityModel;
            var selectedItemOfflineMode = e.Item as MenuItemEntity;


            if (!selectedItemOfflineMode.IsMenuEnabledAsBool)
            {
                return;
            }

            await Navigation.PushAsync(new MainPageDetailOffline(selectedItemOfflineMode));
            ((ListView)sender).SelectedItem = null;
        }
    }
}