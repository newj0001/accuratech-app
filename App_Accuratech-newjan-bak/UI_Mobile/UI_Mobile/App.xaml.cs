using Xamarin.Forms;
using UI_Mobile.Views;
using Common.Model;
using System.Diagnostics;

namespace UI_Mobile
{
    public partial class App : Application
    {
        public static Repo Repository;
        public App(string dbPath)
        {
            InitializeComponent();

            Debug.WriteLine($"Database located at: {dbPath}");

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
