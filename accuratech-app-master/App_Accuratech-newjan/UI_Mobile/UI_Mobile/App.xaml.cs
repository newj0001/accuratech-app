using System;
using Common;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using UI_Mobile.Views;
using UI_Mobile.Models;
using System.Collections.Generic;
using System.Linq;
using SQLitePCL;
using System.Diagnostics;
using System.IO;
using UI_Mobile.Views.Offline;
using Xamarin.Essentials;
using UI_Mobile.ViewModels;
using System.Threading.Tasks;

namespace UI_Mobile
{
    public partial class App : Application
    {
        private static Stopwatch stopWatch = new Stopwatch();
        BackgroundThread backgroundThread = new BackgroundThread();

        static LocalDatabase localDatabase;

        public static string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "MenuItem.db3");

        public static LocalDatabase LocalDatabase
        {
            get
            {
                if (localDatabase == null)
                {
                    localDatabase = new LocalDatabase(dbPath);
                }
                return localDatabase;
            }
        }


        public App()
        {
            InitializeComponent();
            SetMainPage();
        }

        private void SetMainPage()
        {
              MainPage = new MainPageOffline();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
            if (!stopWatch.IsRunning)
            {
                stopWatch.Start();
            }

            Device.StartTimer(new TimeSpan(0, 0, 10), () =>
            {

                Debug.WriteLine("Sending local registrations to server");

                backgroundThread.CheckRegistrationsInQueueAndSendToServer();

                return true;
            });

        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
            stopWatch.Reset();
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
            stopWatch.Start();
        }
    }
}
