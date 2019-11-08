using Common;
using Common.Standard.Persistence;
using Common.Standard.Persistence.Entities;
using System;
using System.Diagnostics;
using UI_Mobile.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace UI_Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPageMaster : ContentPage
    {
        protected readonly DatabaseContextOffline DbContextOffline;
        public MainPageMaster()
        {
            InitializeComponent();
            DbContextOffline = App.DbContext;
            var mainPageMasterViewModel = new MainPageMasterViewModel();
            mainPageMasterViewModel.Reset();
            BindingContext = mainPageMasterViewModel;
        }
        private async void Button_Clicked(object sender, EventArgs e)
        {
            try
            {
                var queue = new QueueItem()
                {
                    Id = 1,
                    Url = "Test",
                    Body = "test",
                    Date = "test2"
                };

                DbContextOffline.QueueItems.Add(queue);
                await DbContextOffline.SaveChangesAsync();
                await DisplayAlert("Db", "Created", "ok");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.InnerException.Message);
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