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

namespace UI_Mobile
{
    public partial class App : Application
    {
        static QueueDatabase queueDatabase;
        static MenuItemDatabase menuItemDatabase;

        public static string FilePath;

        public static QueueDatabase QueueDatabase
        {
            get
            {
                if (queueDatabase == null)
                {
                    queueDatabase = new QueueDatabase(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Queue.db3"));
                }
                return queueDatabase;
            }
        }

        public static MenuItemDatabase MenuItemDatabase
        {
            get
            {
                if (menuItemDatabase == null)
                {
                    menuItemDatabase = new MenuItemDatabase(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "MenuItem.db3"));
                }
                return menuItemDatabase;
            }
        }

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
