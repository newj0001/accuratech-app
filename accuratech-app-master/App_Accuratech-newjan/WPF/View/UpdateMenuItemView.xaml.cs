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
    public partial class UpdateMenuItemView : Window
    {
        public UpdateMenuItemView()
        {
            InitializeComponent();
            DataContextChanged += Reset;
        }
        public UpdateMenuItemViewModel UpdateMenuItemViewModel => DataContext as UpdateMenuItemViewModel;

        public async void Reset(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (UpdateMenuItemViewModel != null) await UpdateMenuItemViewModel?.Reset();
        }

        private async void btnSaveMenuItem_Click(object sender, RoutedEventArgs e)
        {
            
            await UpdateMenuItemViewModel.UpdateMenuItem();
            Close();
        }

        private void btnCancelMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
//    var editGeneralFieldSettingsViewModel = new EditGeneralFieldSettingsViewModel(GetFieldItemButton(sender, e));
//    editGeneralFieldSettingsViewModel.SubItemUpdated += async (_, __) => await MainWindowViewModel?.Reset();
//    var editGeneralFieldSettingsView = new EditGeneralFieldSettingsView { DataContext = editGeneralFieldSettingsViewModel };
//    Main.Content = editGeneralFieldSettingsView;