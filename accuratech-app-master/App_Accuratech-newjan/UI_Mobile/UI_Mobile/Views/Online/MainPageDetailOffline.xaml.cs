using Common;
using Common.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UI_Mobile.Models;
using UI_Mobile.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Honeywell.AIDC.CrossPlatform;
using System.Threading;
using Newtonsoft.Json;
using SQLite;

using System.Diagnostics;

namespace UI_Mobile.Views.Online
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPageDetailOffline : ContentPage
    {
        #region BARCODE VARIABLES
        private BarcodeReader mSelectedReader = null;
        private const string DEFAULT_READER_KEY = "default";
        private Dictionary<string, BarcodeReader> mBarcodeReaders;
        private SynchronizationContext mUIContext = SynchronizationContext.Current;
        private bool mSoftContinuousScanStarted = false;
        private bool mSoftOneShotScanStarted = false;
        private string _scannerName;
        #endregion

        private MenuItemEntity _parentMenuItem;

        private readonly FieldItemDataStore _fieldItemDataStore = new FieldItemDataStore();
        private readonly RegistrationDataStore _registrationDataStore = new RegistrationDataStore();

        public MainPageDetailOffline(MenuItemEntity menuItemEntity)
        {
            InitializeComponent();
            MainPageDetailViewModelOffline mainPageDetailViewModelOffline = new MainPageDetailViewModelOffline();
            mainPageDetailViewModelOffline.Reset(menuItemEntity);
            BindingContext = mainPageDetailViewModelOffline;
            mBarcodeReaders = new Dictionary<string, BarcodeReader>();
            _parentMenuItem = menuItemEntity;
        }

        protected async override void OnAppearing()
        {
            PopulateReaderPicker();
            await OpenBarcodeReader();
            await ToogleBarcodeReader(true);
            await SetConnectivityLabel();
            ClearText(_parentMenuItem);
        }
        protected async override void OnDisappearing()
        {
            await CloseBarcodeReader();
        }

        #region SCANNER FUNCTIONS

        #region START & STOP BARCODE SCAN
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
        #endregion

        #region OPEN & CLOSE BARCODE READER
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
        #endregion

        #region BARCODE READER
        private void MBarcodeReader_BarcodeDataReady(object sender, BarcodeDataArgs e)
        {
            mUIContext.Post(_ =>
            {
                UpdateBarcodeInfo(e.Data, _parentMenuItem);
            }, null);
        }
        private void UpdateBarcodeInfo(string data, MenuItemEntity menuItemEntity)
        {
            var scanData = data;

            foreach (var item in menuItemEntity.SubItems)
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
        #endregion

        #region SET SCANNER AND SYMBOLOGYSETTINGS
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

        #endregion

        #endregion

        #region UI FUNCTIONS
        private async Task SetConnectivityLabel()
        {
            var current = Connectivity.NetworkAccess;
            if (current == NetworkAccess.Internet)
            {
                await LabelConnection.FadeTo(0).ContinueWith((result) => { });
            }
            else
            {
                await LabelConnection.FadeTo(1).ContinueWith((result) => { });
            }
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

        public void ClearText(MenuItemEntity menuItemEntity)
        {
            foreach (var item in menuItemEntity.SubItems)
            {
                item.FieldValue = "";
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

        public void UpdateText(MenuItemEntity menuItemEntity, string text)
        {
            foreach (var item in menuItemEntity.SubItems)
            {
                item.FieldValue = text;
            }
        }
        #endregion

        #region SAVE TO SQLITE LOCAL DATABASEQUEUE

        private async void SaveClicked(object sender, EventArgs e)
         {
            RegistrationModel registrationOffline = SaveRegistrations();
            QueueItem queueItem = InsertIntoQueue(registrationOffline);
            await InsertRegistrationIntoQueue(registrationOffline, queueItem);
        }

        private async Task InsertRegistrationIntoQueue(RegistrationModel registration, QueueItem queueItem)
        {
            // GEM I KØ
            await App.LocalDatabase.SaveQueueItem(queueItem);
            Thread thread = new Thread(CheckQueueItemsAndSendToServer);
            thread.Start();
            for (int i = 0; i < 1000; i++)
            {
                Debug.WriteLine("Working thread...");
                Thread.Sleep(100);
            }
        }

        private async void CheckQueueItemsAndSendToServer(object registration)
        {
            try
            {
                var current = Connectivity.NetworkAccess;
                if (current == NetworkAccess.Internet)
                {
                    await _registrationDataStore.AddItemAsync((RegistrationModel)registration);

                    var entities = App.LocalDatabase.FetchQueueItems();

                    foreach (var entity in entities)
                    {
                        try
                        {
                            await App.LocalDatabase.DeleteQueueItemAsync(entity.Id);
                        }
                        catch (Exception)
                        {

                            break;
                        }
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }

            for (int i = 0; i < 1000; i++)
            {
                Debug.WriteLine("Working thread...");
                Thread.Sleep(100);
            }
        }

        //private async Task CheckItemInQueue(RegistrationModel registration)
        //{
        //    // SEND TIL SERVER
        //    await _registrationDataStore.AddItemAsync(registration);

        //    //PRØV IGEN SENERE
        //    if (current == NetworkAccess.None)
        //    {
        //        var itemsOnline = _registrationDataStore.AddItemAsync(registration);
        //        if (itemsOnline != null)
        //        {
        //            var entities = App.LocalDatabase.FetchQueueItems();
        //            foreach (var entity in entities)
        //            {
        //                try
        //                {
        //                    await App.LocalDatabase.DeleteQueueItemAsync(entity.Id);
        //                }
        //                catch (SQLiteException)
        //                {
        //                    break;
        //                }
        //            }
        //        }
        //    }
        //}

        private RegistrationModel SaveRegistrations()
        {
            var subItemsOffline = ((ListView)SubItemsListView).ItemsSource;
            var registrationOffline = new RegistrationModel { MenuItemId = _parentMenuItem.Id };

            foreach (var item in subItemsOffline)
            {
                SubItemEntity subItemEntity = (SubItemEntity)item;
                var mainPageDetailViewModel = new MainPageDetailViewModelOffline(subItemEntity);

                var registrationValue = new RegistrationValueModel();
                registrationValue.SubItemId = subItemEntity.Id;
                registrationValue.Value = subItemEntity.FieldValue;
                registrationValue.SubItemName = subItemEntity.Name;
                registrationOffline.RegistrationValues.Add(registrationValue);
            }

            return registrationOffline;
        }

        private static QueueItem InsertIntoQueue(RegistrationModel registration)
        {
            return new QueueItem()
            {
                Url = "http://172.30.1.141:44333/api/registration/",
                Body = JsonConvert.SerializeObject(registration),
                Date = DateTime.UtcNow
            };
        }

        #endregion


        //private ICollection<SubItemEntity> ConvertToEntity(ICollection<SubItemEntityModel> items)
        //{
        //    var entities = new List<SubItemEntity>();
        //    foreach (var item in items)
        //    {
        //        var x = new SubItemEntity();
        //        x.Id = item.Id;
        //        x.IsFieldEnabled = item.IsFieldEnabled;
        //        x.IsFieldEnabledAsBool = item.IsFieldEnabledAsBool;
        //        x.IsNumericFieldEnabled = item.IsNumericFieldEnabled;
        //        x.IsScanEnabled = item.IsScanEnabled;
        //        x.KeepFieldValue = item.KeepFieldValue;
        //        x.KeyboardInput = item.KeyboardInput;
        //        x.Length = item.Length;
        //        x.MenuItemId = item.MenuItemId;
        //        x.Name = item.Name;
        //        x.NumericFieldEnabled = item.NumericFieldEnabled;
        //        x.Offset = item.Offset;
        //        x.ScanEnabled = item.ScanEnabled;
        //        x.StartWith = item.StartWith;
        //        x.Type = item.Type;
        //        x.ValueLength = item.ValueLength;
        //        entities.Add(x);
        //    }
        //    return entities;
        //}
    }
}