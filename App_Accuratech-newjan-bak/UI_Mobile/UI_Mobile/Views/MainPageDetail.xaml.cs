using Common;
using Honeywell.AIDC.CrossPlatform;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using UI_Mobile.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Common.Model;

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
        private readonly HttpClient _apiClient;


        public MainPageDetail(MenuItemEntityModel menuItemEntityModel)
        {
            InitializeComponent();
            _apiClient = ApiHelper.GetApiClient();
            MainPageDetailViewModel mainPageDetailViewModel = new MainPageDetailViewModel();
            mainPageDetailViewModel.Reset(menuItemEntityModel);
            BindingContext = mainPageDetailViewModel;
            mBarcodeReaders = new Dictionary<string, BarcodeReader>();
            _parentMenuItem = menuItemEntityModel;
        }

        private async void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {

            //if (e.NetworkAccess == NetworkAccess.Internet)
            //{
            //    await LabelConnection.FadeTo(0).ContinueWith((result) => { });
            //}
            //else
            //{
            //    await LabelConnection.FadeTo(1).ContinueWith((result) => { });
            //    try
            //    {
            //        //upload body og url
            //    }
            //    catch (Exception ex)
            //    {

            //        QueueEntity entity = new QueueEntity()
            //        {
            //            Url = "test",
            //            Body = "test",
            //            Date = DateTime.Now
            //        };
            //        RestQueue.Insert(entity);
            //    }

            //    await DisplayAlert("Success", "Added Successfully", "OK");


            //    //Sync when back online
            //    //Run at app startup or every x minutes
            //    var entities = RestQueue.Fetch();
            //    foreach (var entity in entities)
            //    {
            //        try
            //        {
            //            //request server //upload body og url

            //            //
            //            RestQueue.Delete(entity.Id);
            //        }
            //        catch (Exception ex)
            //        {
            //            // No connection. Try again later...
            //            break;
            //        }
            //    }
            //}
        }

        //private async void SaveClicked(object sender, EventArgs e)
        //{
        //    var subItems = ((ListView)SubItemsListView).ItemsSource;
        //    var registration = new RegistrationModel { MenuItemId = _parentMenuItem.Id };

        //    foreach (var item in subItems)
        //    {
        //        SubItemEntityModel subItemEntity = (SubItemEntityModel)item;
        //        var fieldItemViewModel = new MainPageDetailViewModel(subItemEntity);

        //        var registrationValue = new RegistrationValueModel();
        //        registrationValue.SubItemId = subItemEntity.Id;
        //        registrationValue.Value = subItemEntity.FieldValue;
        //        registrationValue.SubItemName = subItemEntity.Name;

        //        registration.RegistrationValues.Add(registrationValue);
        //    }
        //    await new RegistrationDataStore().AddItemAsync(registration);
        //}

        //var url = "http://localhost:44333/api/menuitem";
        //try
        //{
        //    using (HttpResponseMessage response = await _apiClient.GetAsync(url))
        //    {
        //        if (response.IsSuccessStatusCode)
        //        {
        //            await response.Content.ReadAsAsync<List<MenuItemEntityModel>>();
        //        }
        //        else
        //        {
        //            throw new Exception(response.ReasonPhrase);
        //        }
        //    }
        //}
        //catch (Exception)
        //{
        //    HttpResponseMessage response = await _apiClient.GetAsync(url);
        //    if (response.IsSuccessStatusCode)
        //    {
        //        MenuItemEntityModel menuItem = await response.Content.ReadAsAsync<MenuItemEntityModel>();
        //        var entity = new RestQueueEntity()
        //        {
        //            Url = url,
        //            Body = menuItem.ToString(),
        //            Date = DateTime.Now
        //        };
        //        RestQueue.Insert(entity);
        //    }
        //    throw;
        //}

        #region SCAN
        protected override async void OnAppearing()
        {
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
            PopulateReaderPicker();
            await OpenBarcodeReader();
            await ToogleBarcodeReader(true);
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
            //await new RegistrationDataStore().AddItemAsync(registration);

            var queue = new QueueEntity
            {
                Url = "test",
                Body = "test",
                Date = DateTime.Now
            };


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