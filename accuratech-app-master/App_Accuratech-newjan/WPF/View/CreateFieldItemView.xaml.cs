using Common.ViewModel;
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
    public partial class CreateFieldItemView : Window
    {

        public CreateFieldItemView()
        {
            InitializeComponent();
        }
        CreateFieldItemViewModel CreateFieldItemViewModel => DataContext as CreateFieldItemViewModel;

        private void btnCancelMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private async void btnSaveMenuItem_Click(object sender, RoutedEventArgs e)
        {
            await CreateFieldItemViewModel.AddFieldItem();
            Close();
        }
    }
}
