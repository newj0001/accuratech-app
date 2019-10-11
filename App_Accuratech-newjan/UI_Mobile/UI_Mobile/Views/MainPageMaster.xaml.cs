using Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
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