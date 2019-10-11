using System;
using Common;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using UI_Mobile.Views;

namespace UI_Mobile
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();
            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
