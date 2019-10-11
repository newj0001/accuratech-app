using Common;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
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
using Common.ViewModel;
using WPF.View;
using WPF.ViewModels;

namespace WPF
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContextChanged += Reset;
            DataContext = new MainWindowViewModel();
        }

        private async void Reset(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (MainWindowViewModel != null) await MainWindowViewModel?.Reset();
        }

        MainWindowViewModel MainWindowViewModel => DataContext as MainWindowViewModel;

        private void ButtonPopUpLogout_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void BtnAddMenu_Click(object sender, RoutedEventArgs e)
        {
            var createMenuItemViewModel = new CreateMenuItemViewModel();
            createMenuItemViewModel.NewMenuItemCreated += async (_, __) => await MainWindowViewModel?.Reset();
            var createMenuItemView = new CreateMenuItemView() { DataContext = createMenuItemViewModel };
            createMenuItemView.Show();
        }

        private void BtnMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            var editGeneralMenuSettingsViewModel = new GeneralMenuSettingsViewModel(GetMenuItemButton(sender, e));
            editGeneralMenuSettingsViewModel.MenuItemUpdated += async (_, __) => await MainWindowViewModel?.Reset();
            var editGeneralMenuSettingsView = new GeneralMenuSettingsView(GetMenuItemButton(sender, e)) { DataContext = editGeneralMenuSettingsViewModel };
            Main.Content = editGeneralMenuSettingsView;
        }

        private void BtnUpdateMenu_Click(object sender, RoutedEventArgs e)
        {
            var updateMenuItemViewModel = new UpdateMenuItemViewModel(GetMenuItemButton(sender, e));
            updateMenuItemViewModel.MenuItemUpdated += async (_, __) => await MainWindowViewModel?.Reset();
            var updateMenuItemView = new UpdateMenuItemView { DataContext = updateMenuItemViewModel };
            updateMenuItemView.Show();
        }

        private void BtnUpdateField_Click(object sender, RoutedEventArgs e)
        {
            var updateFieldItemViewModel = new UpdateFieldItemViewModel(GetFieldItemButton(sender, e));
            updateFieldItemViewModel.SubItemUpdated += async (_, __) => await MainWindowViewModel?.Reset();
            var updateFieldItemView = new UpdateFieldItemView { DataContext = updateFieldItemViewModel };
            updateFieldItemView.Show();
        }


        private void BtnAddField_Click(object sender, RoutedEventArgs e)
        {
            var createFieldItemViewModel = new CreateFieldItemViewModel(GetMenuItemButton(sender, e));
            createFieldItemViewModel.NewSubItemCreated += async (_, __) => await MainWindowViewModel?.Reset();
            var createFieldItemView = new CreateFieldItemView { DataContext = createFieldItemViewModel };
            createFieldItemView.Show();
        }

        public MenuItemEntityModel GetMenuItemButton(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var menuItem = (MenuItemEntityModel)button.DataContext;
            return menuItem;
        }

        public SubItemEntityModel GetFieldItemButton(object sender, RoutedEventArgs e)
        {
            var button = (Button) sender;
            var fieldItem = (SubItemEntityModel) button.DataContext;
            return fieldItem;
        }
    }
}
