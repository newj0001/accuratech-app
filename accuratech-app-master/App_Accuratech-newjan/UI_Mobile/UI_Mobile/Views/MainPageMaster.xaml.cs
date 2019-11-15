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
            MenuListView.ItemsSource = null;

            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
            var current = Connectivity.NetworkAccess;
            if (current != NetworkAccess.None)
            {
                await LabelConnection.FadeTo(0).ContinueWith((result) => { });
                var itemsOnline = await _menuItemDataStore.GetItemsAsync();
                if (itemsOnline != null)
                {
                    await App.LocalDatabase.DeleteAllMenuItemAsync();
                    await App.LocalDatabase.SaveMenuItemsAsync(ConvertToEntity(itemsOnline));
                    MenuListView.ItemsSource = itemsOnline;
                }
            }

            if (MenuListView.ItemsSource == null)
            {
                var itemsOffline = App.LocalDatabase.LoadMenuItemsAsync();
                MenuListView.ItemsSource = await itemsOffline;
                await LabelConnection.FadeTo(1).ContinueWith((result) => { });
            }
        }

        protected override void OnDisappearing()
        {
            Connectivity.ConnectivityChanged -= Connectivity_ConnectivityChanged;
        }

        private async void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
         {
            if (e.NetworkAccess == NetworkAccess.None)
            {
                await LabelConnection.FadeTo(1).ContinueWith((result) => { });
            }
            else
            {
                await LabelConnection.FadeTo(0).ContinueWith((result) => { });
            }
        }

        private async void OnItemSelected(object sender, ItemTappedEventArgs e)
        {
            var current = Connectivity.NetworkAccess;
            var selectedItem = e.Item as MenuItemEntityModel;
            var selectedItemOffline = e.Item as MenuItemEntity; 

            if (current == NetworkAccess.Internet)
            {
                if (!selectedItem.IsMenuEnabledAsBool)
                {
                    return;
                }
                await Navigation.PushAsync(new MainPageDetail(selectedItem));
                ((ListView)sender).SelectedItem = null;
            }
            else if (current == NetworkAccess.None)
            {
                if (!selectedItemOffline.IsMenuEnabledAsBool)
                {
                    return;
                }
                await Navigation.PushAsync(new MainPageDetailOffline(selectedItemOffline));
                ((ListView)sender).SelectedItem = null;
            }
        }

        private ICollection<MenuItemEntity> ConvertToEntity(ICollection<MenuItemEntityModel> items)
        {
            var entities = new List<MenuItemEntity>();
            foreach (var item in items)
            {
                var x = new MenuItemEntity();
                x.Id = item.Id;
                x.Header = item.Header;
                x.IsMenuEnabled = item.IsMenuEnabled;
                x.IsMenuEnabledAsBool = item.IsMenuEnabledAsBool;
                x.SubItems = ConvertToSubListEntity(item.SubItems);

                entities.Add(x);
            }

            return entities;
        }

        //private List<RegistrationItemEntity> ConvertToRegEntity(ICollection<RegistrationModel> registrations)
        //{
        //    var entities = new List<RegistrationItemEntity>();
        //    foreach (var item in registrations)
        //    {
        //        var x = new RegistrationItemEntity();
        //        {
        //            x.Id = item.Id;
        //            x.MenuItemId = item.MenuItemId;
        //            x.RegistrationValues = ConvertToRegValueEntity(item.RegistrationValues);
        //            x.Timestamp = item.Timestamp;

        //            entities.Add(x);
        //        }
        //    }
        //    return entities;
        //}


        //private List<RegistrationValueItemEntity> ConvertToRegValueEntity(ICollection<RegistrationValueModel> registrationValues)
        //{
        //    var entities = new List<RegistrationValueItemEntity>();
        //    foreach (var item in registrationValues)
        //    {
        //        var x = new RegistrationValueItemEntity();
        //        {
        //            x.Id = item.Id;
        //            x.RegistrationId = item.RegistrationId;
        //            x.SubItemId = item.SubItemId;
        //            x.SubItemName = item.SubItemName;
        //            x.Value = item.Value;

        //            entities.Add(x);
        //        }
        //    }
        //    return entities;
        //}

        private List<SubItemEntity> ConvertToSubListEntity(ICollection<SubItemEntityModel> subItems)
        {
            var entities = new List<SubItemEntity>();
            foreach (var item in subItems)
            {
                var x = new SubItemEntity();
                {
                    x.Id = item.Id;
                    x.IsFieldEnabled = item.IsFieldEnabled;
                    x.IsFieldEnabledAsBool = item.IsFieldEnabledAsBool;
                    x.IsNumericFieldEnabled = item.IsNumericFieldEnabled;
                    x.IsScanEnabled = item.IsScanEnabled;
                    x.KeepFieldValue = item.KeepFieldValue;
                    x.KeyboardInput = item.KeyboardInput;
                    x.Length = item.Length;
                    x.MenuItemId = item.MenuItemId;
                    x.Name = item.Name;
                    x.NumericFieldEnabled = item.NumericFieldEnabled;
                    x.Offset = item.Offset;
                    x.ScanEnabled = item.ScanEnabled;
                    x.StartWith = item.StartWith;
                    x.Type = item.Type;
                    x.ValueLength = item.ValueLength;

                    entities.Add(x);
                }
            }
            return entities;
        }

       
    }
}
