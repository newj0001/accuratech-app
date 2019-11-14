using Common;
using Common.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UI_Mobile.Models;
using UI_Mobile.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace UI_Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPageDetailOffline : ContentPage
    {
        private MenuItemEntity _parentMenuItem;
        private readonly FieldItemDataStore _fieldItemDataStore = new FieldItemDataStore();
        public MainPageDetailOffline(MenuItemEntity menuItemEntity)
        {
            InitializeComponent();
            MainPageDetailViewModelOffline mainPageDetailViewModelOffline = new MainPageDetailViewModelOffline();
            mainPageDetailViewModelOffline.Reset(menuItemEntity);
            BindingContext = mainPageDetailViewModelOffline;
            _parentMenuItem = menuItemEntity;
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;

            var current = Connectivity.NetworkAccess;
            if (current == NetworkAccess.None)
            {
                SubItemsListView.ItemsSource = await App.MenuItemDatabase.GetSubItemsAsync(_parentMenuItem.SubItems);
                await LabelConnection.FadeTo(1).ContinueWith((result) => { });
            }
            else
            {
                SubItemsListView.ItemsSource = await _fieldItemDataStore.GetItemsAsync();
                await LabelConnection.FadeTo(0).ContinueWith((result) => { });
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
                var itemsOffline = App.MenuItemDatabase.GetSubItemsAsync(_parentMenuItem.SubItems);
                SubItemsListView.ItemsSource = await itemsOffline;
                await LabelConnection.FadeTo(1).ContinueWith((result) => { });
            }
            else
            {
                await LabelConnection.FadeTo(0).ContinueWith((result) => { });
                var items = await _fieldItemDataStore.GetItemsAsync();
                await App.MenuItemDatabase.DeleteAllSubItemAsync();
                await App.MenuItemDatabase.SaveSubItemsAsync(ConvertToEntity(items));
                SubItemsListView.ItemsSource = items;
            }
        }

        private ICollection<SubItemEntity> ConvertToEntity(ICollection<SubItemEntityModel> items)
        {
            var entities = new List<SubItemEntity>();
            foreach (var item in items)
            {
                var x = new SubItemEntity();
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
            return entities;
        }
    }
}