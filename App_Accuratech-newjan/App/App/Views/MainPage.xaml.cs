using Common;
using Common.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            ApiHelper.InitializeClient();
            BindingContextChanged += async (_, __) => await MainWindowViewModel?.Reset();
            BindingContext = new MainWindowViewModel();
        }

        MainWindowViewModel MainWindowViewModel => BindingContext as MainWindowViewModel;

        public async void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var selectedItem = e.Item as MenuItemEntityModel;
            
            await Navigation.PushAsync(new NewItemPage(selectedItem));

            ((ListView)sender).SelectedItem = null;
        }
    }
}