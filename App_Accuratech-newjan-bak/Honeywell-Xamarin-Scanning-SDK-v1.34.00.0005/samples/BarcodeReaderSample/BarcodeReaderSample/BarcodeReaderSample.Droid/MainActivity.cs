using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Content.Res;

namespace BarcodeReaderSample.Droid
{
    [Activity(Label = "BarcodeReaderSample", Icon = "@drawable/icon", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
    {
        private BarcodeReaderSample.App mBarcodeReaderSampleApp;
        private BarcodeReaderSample.MainPage mMainPage;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            Xamarin.Essentials.Platform.Init(this, bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication((mBarcodeReaderSampleApp = new BarcodeReaderSample.App()));
            mMainPage = (BarcodeReaderSample.MainPage)mBarcodeReaderSampleApp.MainPage;

            //lock application orientation
            ScreenLayout screenSize = Application.Context.ApplicationContext.Resources.Configuration.ScreenLayout & ScreenLayout.SizeMask;
            switch (screenSize)
            {
                case ScreenLayout.SizeXlarge:
                case ScreenLayout.SizeLarge:
                    this.RequestedOrientation = ScreenOrientation.Landscape;
                    break;
                default:
                    this.RequestedOrientation = ScreenOrientation.Portrait;
                    break;
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        /// <summary>
        /// When the ScanPage is about to go to the background, release the
        /// scanner.
        /// </summary>
        protected override void OnPause()
        {
            base.OnPause();
            mMainPage.CloseBarcodeReader();
        }

        /// <summary>
        /// When the ScanPage is about to go to the foreground, claim the
        /// scanner.
        /// </summary>
        protected override void OnResume()
        {
            base.OnResume();
            mMainPage.OpenBarcodeReader();
        }
    }
}

