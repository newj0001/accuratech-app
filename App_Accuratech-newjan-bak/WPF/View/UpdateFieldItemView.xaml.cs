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
    /// <summary>
    /// Interaction logic for UpdateFieldItemView.xaml
    /// </summary>
    public partial class UpdateFieldItemView : Window
    {
        public UpdateFieldItemView()
        {
            InitializeComponent();
            DataContextChanged += Reset;
        }

        UpdateFieldItemViewModel UpdateFieldItemViewModel => DataContext as UpdateFieldItemViewModel;

        public async void Reset(object sender, DependencyPropertyChangedEventArgs e)
        {

        }
        private async void btnSaveFieldItem_Click(object sender, RoutedEventArgs e)
        {
            await UpdateFieldItemViewModel.UpdateFieldItem();
            Close();
        }

        private void btnCancelFieldItem_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
