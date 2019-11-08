using System;
using Common;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using UI_Mobile.Views;
using UI_Mobile.Models;
using System.Collections.Generic;
using System.Linq;
using SQLitePCL;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Common.Standard.Persistence;

namespace UI_Mobile
{
    public partial class App : Application
    {
        public static DatabaseContextOffline DbContext { get; private set; }

        public App()
        {
            InitializeComponent();

            InitializeDatabase();

            MainPage = new MainPage();
        }

        private void InitializeDatabase()
        {
            try
            {
                Batteries_V2.Init();
                using var db = DbConnection.Instance.DbContextOffline;
                db.Database.Migrate();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.InnerException.Message);
            }

            DbContext = DbConnection.Instance.DbContextOffline;
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
