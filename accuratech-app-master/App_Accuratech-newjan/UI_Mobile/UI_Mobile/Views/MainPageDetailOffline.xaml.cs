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
        private MenuItemEntity _parentMenuItemOffline;
        public MainPageDetailOffline(MenuItemEntity menuItemEntity)
        {
            InitializeComponent();

            MainPageDetailViewModelOffline mainPageDetailViewModelOffline = new MainPageDetailViewModelOffline();
            mainPageDetailViewModelOffline.Reset(menuItemEntity);
            BindingContext = mainPageDetailViewModelOffline;
            _parentMenuItemOffline = menuItemEntity;
        }

        protected override async void OnAppearing()
        {
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
            ClearText(_parentMenuItemOffline);
        }

        protected override async void OnDisappearing()
        {
            Connectivity.ConnectivityChanged -= Connectivity_ConnectivityChanged;
        }

        private async void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            if (e.NetworkAccess == NetworkAccess.Internet)
            {
                await LabelConnection.FadeTo(0).ContinueWith((result) => { });
            }
            else
            {
                await LabelConnection.FadeTo(1).ContinueWith((result) => { });
            }
        }

        private async void ClearClicked(object sender, EventArgs e)
        {
            bool answer = await DisplayAlert("", $"Would you like to clear the registration?", "Save", "Cancel");
            if (answer)
                ClearText(_parentMenuItemOffline);
            else
                return;
        }

        public void ClearText(MenuItemEntity menuItemEntity)
        {
            foreach (var item in menuItemEntity.SubItems)
            {
                item.FieldValue = "";
            }
        }

        private void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            var selectedItem = e.Item as SubItemEntity;

            if (!selectedItem.IsFieldEnabledAsBool)
            {
                return;
            }
        }

        public void OnScanButtonClicked(object sender, EventArgs args)
        {
            //if (mSelectedReader != null && mSelectedReader.IsReaderOpened)
            //{
            //    if (mSoftOneShotScanStarted || mSoftContinuousScanStarted)
            //    {
            //        StopBarcodeScan();
            //    }
            //    else
            //    {
            //        StartBarcodeScan();
            //    }
            //}
        }

        private async void SaveClicked(object sender, EventArgs e)
        {
            //var subItems = ((ListView)SubItemsListView).ItemsSource;
            //var registration = new RegistrationModel { MenuItemId = _parentMenuItem.Id };

            //foreach (var item in subItems)
            //{
            //    SubItemEntityModel subItemEntity = (SubItemEntityModel)item;
            //    var fieldItemViewModel = new MainPageDetailViewModel(subItemEntity);

            //    var registrationValue = new RegistrationValueModel();
            //    registrationValue.SubItemId = subItemEntity.Id;
            //    registrationValue.Value = subItemEntity.FieldValue;
            //    registrationValue.SubItemName = subItemEntity.Name;

            //    registration.RegistrationValues.Add(registrationValue);
            //}

            //QueueItem queueItem = new QueueItem()
            //{
            //    Url = "http://172.30.1.141:44333/api/registration/",
            //    Body = JsonConvert.SerializeObject(registration),
            //    Date = DateTime.UtcNow
            //};

            //var current = Connectivity.NetworkAccess;

            //if (current == NetworkAccess.Internet)
            //{
            //    var entities = App.QueueDatabase.FetchQueueItems();
            //    foreach (var entity in entities)
            //    {
            //        try
            //        {
            //            await App.QueueDatabase.DeleteQueueItemAsync(entity.Id);
            //        }
            //        catch (Exception)
            //        {

            //            break;
            //        }
            //    }
            //    await new RegistrationDataStore().AddItemAsync(registration);
            //    await DisplayAlert("Ok", "Saved Online", "Ok");
            //}
            //else
            //{
            //    await App.QueueDatabase.SaveQueueItemAsync(queueItem);
            //    await DisplayAlert("Ok", "Saved Offline", "Ok");
            //}
        }
    }
}