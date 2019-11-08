using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WPF.ViewModels;

namespace WPF.View
{
    public partial class CreateMenuItemView : Window
    {
        public CreateMenuItemView()
        {
            InitializeComponent();
        }

        public CreateMenuItemViewModel CreateMenuItemViewModel => DataContext as CreateMenuItemViewModel;

        private async void BtnSaveMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            await CreateMenuItemViewModel.AddMenuItem();
            Close();
        }

        private void BtnCancelMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
