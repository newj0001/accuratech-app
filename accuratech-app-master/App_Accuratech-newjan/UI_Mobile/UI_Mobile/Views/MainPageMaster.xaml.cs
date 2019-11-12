using Common;
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
        public MainPageMaster()
        {
            InitializeComponent();
            var mainPageMasterViewModel = new MainPageMasterViewModel();
            mainPageMasterViewModel.Reset();
            BindingContext = mainPageMasterViewModel;
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