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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Common;
using Common.Services;
using Common.ViewModel;

namespace WPF.View
{
    public partial class GeneralMenuSettingsView : UserControl
    {
        public GeneralMenuSettingsView(MenuItemEntityModel menuItemEntityModel)
        {
            InitializeComponent();
            DataContextChanged += Reset;
            DataContext = new GeneralMenuSettingsViewModel(menuItemEntityModel);
        }

        private async void Reset(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (EditGeneralMenuSettingsViewModel != null) await EditGeneralMenuSettingsViewModel?.Reset();
        }

        GeneralMenuSettingsViewModel EditGeneralMenuSettingsViewModel => DataContext as GeneralMenuSettingsViewModel;

        private void BtnUpdateMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            UpdateMenuItemView updateMenuItemView = new UpdateMenuItemView();
            updateMenuItemView.Show();
        }

        private void BtnAddFieldItem_OnClick(object sender, RoutedEventArgs e)
        {
            CreateFieldItemView createFieldItemView = new CreateFieldItemView();
            createFieldItemView.Show();
        }
    }
}
