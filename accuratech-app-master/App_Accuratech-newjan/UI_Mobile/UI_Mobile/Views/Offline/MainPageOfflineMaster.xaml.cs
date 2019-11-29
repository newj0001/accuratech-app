using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UI_Mobile.Models;
using UI_Mobile.ViewModels;
using UI_Mobile.Views.Online;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace UI_Mobile.Views.Offline
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPageOfflineMaster : ContentPage
    {
        private readonly MenuItemDataStore _menuItemDataStore = new MenuItemDataStore();
        //private MenuItemEntityModel _parentMenuItem;
        MainPageMasterViewModelOffline mainPageMasterViewModelOffline = new MainPageMasterViewModelOffline();

        public MainPageOfflineMaster()
        {
            InitializeComponent();
            BindingContext = mainPageMasterViewModelOffline;
        }

        

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            App.LocalDatabase.someEvent += LocalDatabase_someEvent;


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

        private void LocalDatabase_someEvent(int regCount, int recCount)
        {
            UpdateRegistrationsInQueue();
        }

        private void UpdateRegistrationsInQueue()
        {
            var regCounter = App.LocalDatabase.FetchRegistrationItems().Count;
            var recCounter = App.LocalDatabase.FetchRegistrationValueItems().Count;

            Device.BeginInvokeOnMainThread(() =>
            {
                LabelQueue.Text = $"{regCounter} registrations / {recCounter} records in queue";
            });
        }

        protected override void OnDisappearing()
        {
            Connectivity.ConnectivityChanged -= Connectivity_ConnectivityChanged;
            App.LocalDatabase.someEvent -= LocalDatabase_someEvent;
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
            var selectedItemOffline = e.Item as MenuItemEntity;
            var selectedItemOnline = e.Item as MenuItemEntityModel;
            if (current == NetworkAccess.None)
            {
                if (!selectedItemOffline.IsMenuEnabledAsBool)
                {
                    return;
                }
                await Navigation.PushAsync(new MainPageOfflineDetail(selectedItemOffline));
                ((ListView)sender).SelectedItem = null;
            }
            if (current == NetworkAccess.Internet)
            {
                if (!selectedItemOnline.IsMenuEnabledAsBool)
                {
                    return;
                }
                await Navigation.PushAsync(new MainPageDetail(selectedItemOnline));
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