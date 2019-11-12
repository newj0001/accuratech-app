using Common;
using Common.Services;
using Honeywell.AIDC.CrossPlatform;
using Newtonsoft.Json;
using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UI_Mobile.Models;
using UI_Mobile.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace UI_Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPageDetail : ContentPage, INotifyPropertyChanged
    {
        private MenuItemEntityModel _parentMenuItem;
        private SubItemEntityModel _parentSubItem = new SubItemEntityModel();
        private SynchronizationContext mUIContext = SynchronizationContext.Current;
        private const string DEFAULT_READER_KEY = "default";
        private Dictionary<string, BarcodeReader> mBarcodeReaders;
        private BarcodeReader mSelectedReader = null;
        private bool mSoftContinuousScanStarted = false;
        private bool mSoftOneShotScanStarted = false;
        private string _scannerName;

        public MainPageDetail(MenuItemEntityModel menuItemEntityModel)
        {
            InitializeComponent();

            MainPageDetailViewModel mainPageDetailViewModel = new MainPageDetailViewModel();
            mainPageDetailViewModel.Reset(menuItemEntityModel);
            BindingContext = mainPageDetailViewModel;
            mBarcodeReaders = new Dictionary<string, BarcodeReader>();
            _parentMenuItem = menuItemEntityModel;
        }

        #region SCAN
        protected override async void OnAppearing()
        {
            PopulateReaderPicker();
            await OpenBarcodeReader();
            await ToogleBarcodeReader(true);
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
            ClearText(_parentMenuItem);
        }

        protected override async void OnDisappearing()
        {
            Connectivity.ConnectivityChanged -= Connectivity_ConnectivityChanged;
            await CloseBarcodeReader();
        }

        private void MBarcodeReader_BarcodeDataReady(object sender, BarcodeDataArgs e)
        {
            mUIContext.Post(_ =>
            {
                UpdateBarcodeInfo(e.Data, _parentMenuItem);
            }, null);
        }

        private async void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            if (e.NetworkAccess == NetworkAccess.Internet)
            {
                await LabelConnection.FadeTo(0).ContinueWith((result) => { });
                           }
            else
            {
                await LabelConnection.FadeTo(1).ContinueWith((result) => { });
            }
        }

        public async Task ToogleBarcodeReader(bool enable)
        {
            if (mSelectedReader != null && mSelectedReader.IsReaderOpened)
            {
                BarcodeReader.Result result = await mSelectedReader.EnableAsync(enable); // Enables or disables barcode reader
                if (result.Code != BarcodeReader.Result.Codes.SUCCESS)
                {
                    await DisplayAlert("Error", "EnableAsync failed, Code:" + result.Code +
                                        " Message:" + result.Message, "OK");
                }
            }
        }

        public async void StartBarcodeScan()
        {
            if (mSelectedReader != null && mSelectedReader.IsReaderOpened)
            {
                BarcodeReader.Result result = await mSelectedReader.SoftwareTriggerAsync(true);
                if (result.Code == BarcodeReader.Result.Codes.SUCCESS)
                {
                    // Set mSoftOneShotScanStarted to true if not in continuous scan mode.
                    // The mSoftOneShotScanStarted flag is used to turn off the software
                    // trigger after a barcode is read successfully.
                    mSoftOneShotScanStarted = !mSoftContinuousScanStarted;
                }
                else
                {
                    await DisplayAlert("Error", "Failed to turn on software trigger, Code:" + result.Code +
                        " Message:" + result.Message, "OK");
                }
            } //endif (mReaderOpened)
        }

        public async void StopBarcodeScan()
        {
            if (mSelectedReader != null && mSelectedReader.IsReaderOpened)
            {
                if (mSoftOneShotScanStarted || mSoftContinuousScanStarted)
                {
                    // Turn off the software trigger.
                    await mSelectedReader.SoftwareTriggerAsync(false);
                    mSoftOneShotScanStarted = false;
                    mSoftContinuousScanStarted = false;
                }
            }
        }

        public async Task OpenBarcodeReader()
        {
            mSelectedReader = GetBarcodeReader(_scannerName);
            if (!mSelectedReader.IsReaderOpened)
            {
                BarcodeReader.Result result = await mSelectedReader.OpenAsync();

                if (result.Code == BarcodeReader.Result.Codes.SUCCESS ||
                    result.Code == BarcodeReader.Result.Codes.READER_ALREADY_OPENED)
                {
                    SetScannerAndSymbologySettings();
                }
                else
                {
                    await DisplayAlert("Error", "OpenAsync failed, Code:" + result.Code +
                        " Message:" + result.Message, "OK");
                }
            }
        }

        public async Task CloseBarcodeReader()
        {
            if (mSelectedReader != null && mSelectedReader.IsReaderOpened)
            {
                if (mSoftOneShotScanStarted || mSoftContinuousScanStarted)
                {
                    // Turn off the software trigger.
                    await mSelectedReader.SoftwareTriggerAsync(false);
                    mSoftOneShotScanStarted = false;
                    mSoftContinuousScanStarted = false;
                }

                BarcodeReader.Result result = await mSelectedReader.CloseAsync();
                if (result.Code == BarcodeReader.Result.Codes.SUCCESS)
                {

                }
                else
                {
                    await DisplayAlert("Error", "CloseAsync failed, Code:" + result.Code +
                        " Message:" + result.Message, "OK");
                }
            }
        }

        private async void PopulateReaderPicker()
        {
            try
            {
                // Queries the list of readers that are connected to the mobile computer.
                IList<BarcodeReaderInfo> readerList = await BarcodeReader.GetConnectedBarcodeReaders();
                if (readerList.Count > 0)
                {
                    foreach (BarcodeReaderInfo reader in readerList)
                    {
                        _scannerName = reader.ScannerName;
                        break;
                    }
                }
                else
                {
                    _scannerName = "";
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Failed to query connected readers, " + ex.Message, "OK");
            }
        }

        private async void SetScannerAndSymbologySettings()
        {
            try
            {
                if (mSelectedReader.IsReaderOpened)
                {
                    Dictionary<string, object> settings = new Dictionary<string, object>()
                    {
                        {mSelectedReader.SettingKeys.TriggerScanMode, mSelectedReader.SettingValues.TriggerScanMode_OneShot },
                        {mSelectedReader.SettingKeys.Code128Enabled, true },
                        {mSelectedReader.SettingKeys.Code39Enabled, true },
                        {mSelectedReader.SettingKeys.Ean8Enabled, true },
                        {mSelectedReader.SettingKeys.Ean8CheckDigitTransmitEnabled, true },
                        {mSelectedReader.SettingKeys.Ean13Enabled, true },
                        {mSelectedReader.SettingKeys.Ean13CheckDigitTransmitEnabled, true },
                        {mSelectedReader.SettingKeys.Interleaved25Enabled, true },
                        {mSelectedReader.SettingKeys.Interleaved25MaximumLength, 100 },
                        {mSelectedReader.SettingKeys.Postal2DMode, mSelectedReader.SettingValues.Postal2DMode_Usps }
                    };

                    BarcodeReader.Result result = await mSelectedReader.SetAsync(settings);
                    if (result.Code != BarcodeReader.Result.Codes.SUCCESS)
                    {
                        await DisplayAlert("Error", "Symbology settings failed, Code:" + result.Code +
                                            " Message:" + result.Message, "OK");
                    }
                }
            }
            catch (Exception exp)
            {
                await DisplayAlert("Error", "Symbology settings failed. Message: " + exp.Message, "OK");
            }
        }

        public BarcodeReader GetBarcodeReader(string readerName)
        {
            BarcodeReader reader = null;

            if (readerName == DEFAULT_READER_KEY)
            { // This name was added to the Open Reader picker list if the
              // query for connected barcode readers failed. It is not a
              // valid reader name. Set the readerName to null to default
              // to internal scanner.
                readerName = null;
            }

            if (null == readerName)
            {
                if (mBarcodeReaders.ContainsKey(DEFAULT_READER_KEY))
                {
                    reader = mBarcodeReaders[DEFAULT_READER_KEY];
                }
            }
            else
            {
                if (mBarcodeReaders.ContainsKey(readerName))
                {
                    reader = mBarcodeReaders[readerName];
                }
            }

            if (null == reader)
            {
                // Create a new instance of BarcodeReader object.
                reader = new BarcodeReader(readerName);

                // Add an event handler to receive barcode data.
                // Even though we may have multiple reader sessions, we only
                // have one event handler. In this app, no matter which reader
                // the data come frome it will update the same UI controls.
                reader.BarcodeDataReady += MBarcodeReader_BarcodeDataReady;

                // Add the BarcodeReader object to mBarcodeReaders collection.
                if (null == readerName)
                {
                    mBarcodeReaders.Add(DEFAULT_READER_KEY, reader);
                }
                else
                {
                    mBarcodeReaders.Add(readerName, reader);
                }
            }

            return reader;
        }

        public void OnScanButtonClicked(object sender, EventArgs args)
        {
            if (mSelectedReader != null && mSelectedReader.IsReaderOpened)
            {
                if (mSoftOneShotScanStarted || mSoftContinuousScanStarted)
                {
                    StopBarcodeScan();
                }
                else
                {
                    StartBarcodeScan();
                }
            }
        }

        #endregion
        
        #region UI

        private void NumericField(MenuItemEntityModel menuItemEntityModel)
        {
            foreach (var item in menuItemEntityModel.SubItems)
            {
                if (item.NumericFieldEnabled)
                {
                    DisplayAlert("", "Only Number", "OK", "Cancel");
                }
            }
        }

        private void UpdateBarcodeInfo(string data, MenuItemEntityModel menuItemEntityModel)
        {
            var scanData = data;

            foreach (var item in menuItemEntityModel.SubItems)
            {
                if (item.ScanEnabled)
                {
                    if (scanData.Length == item.Length && scanData.StartsWith(item.StartWith))
                    {
                        var resultData = scanData.Substring(item.Offset, item.ValueLength);
                       
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            item.FieldValue = resultData;
                        });
                    }
                }
            }
        }

        private void CheckLength(string data, MenuItemEntityModel menuItemEntityModel)
        {
            var scanData = data;

            foreach (var item in menuItemEntityModel.SubItems)
            {
                //if (scanData.Length <)
                //{

                //}  
            }
        }

        private void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            var selectedItem = e.Item as SubItemEntityModel;
            
            if (!selectedItem.IsFieldEnabledAsBool)
            {
                return;
            }
        }


        private async void SaveClicked(object sender, EventArgs e)
         {
            var subItems = ((ListView)SubItemsListView).ItemsSource;
            var registration = new RegistrationModel { MenuItemId = _parentMenuItem.Id };

            foreach (var item in subItems)
            {
                SubItemEntityModel subItemEntity = (SubItemEntityModel)item;
                var fieldItemViewModel = new MainPageDetailViewModel(subItemEntity);

                var registrationValue = new RegistrationValueModel();
                registrationValue.SubItemId = subItemEntity.Id;
                registrationValue.Value = subItemEntity.FieldValue;
                registrationValue.SubItemName = subItemEntity.Name;

                registration.RegistrationValues.Add(registrationValue);
            }

            QueueItem queueItem = new QueueItem()
            {
                Url = "http://172.30.1.141:44333/api/registration/",
                Body = JsonConvert.SerializeObject(registration),
                Date = DateTime.UtcNow
            }; 

            var current = Connectivity.NetworkAccess;

            if (current == NetworkAccess.Internet)
            {
                await LabelConnection.FadeTo(0).ContinueWith((result) => { });
                await new RegistrationDataStore().AddItemAsync(registration);
                var entities = App.Database.FetchQueueItems();
                foreach (var entity in entities)
                {
                    try
                    {
                        await App.Database.DeleteQueueItemAsync(entity.Id);
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                }
                await DisplayAlert("Ok", "Saved Online", "Ok");
            }
            else
            {
                await LabelConnection.FadeTo(1).ContinueWith((result) => { });
                await App.Database.SaveQueueItemAsync(queueItem);
                await DisplayAlert("Ok", "Saved Offline", "Ok");
            }
        }


        private async void ClearClicked(object sender, EventArgs e)
        {
            bool answer = await DisplayAlert("", $"Would you like to clear the registration?", "Save", "Cancel");
            if (answer)
                ClearText(_parentMenuItem); 
            else
                return;
         }

        public void ClearText(MenuItemEntityModel menuItemEntityModel)
        {
            foreach (var item in menuItemEntityModel.SubItems)
            {
                item.FieldValue = "";
            }
        }

        public void UpdateText(MenuItemEntityModel menuItemEntityModel, string text)
        {
            foreach (var item in menuItemEntityModel.SubItems)
            {
                item.FieldValue = text;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    #endregion

}