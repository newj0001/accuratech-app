using Common;
using Common.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UI_Mobile.Models;
using Xamarin.Essentials;

namespace UI_Mobile
{
    public class BackgroundThread
    {
        private readonly RegistrationDataStore _registrationDataStore = new RegistrationDataStore();
        public async void CheckRegistrationsInQueueAndSendToServer()
        {
            try
            {
                var current = Connectivity.NetworkAccess;
                if (current == NetworkAccess.Internet)
                {
                    await AddLocalRegistrationsToServer();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private async Task<List<RegistrationModel>> AddLocalRegistrationsToServer()
        {
            var localRegistrations = App.LocalDatabase.FetchRegistrationItems();

            foreach (var item in localRegistrations)
            {
                RegistrationModel registrationModel = new RegistrationModel();
                registrationModel.Id = item.Id;
                registrationModel.MenuItemId = item.MenuItemId;
                registrationModel.Timestamp = item.Timestamp;
                registrationModel.RegistrationValues = item.RegistrationValues;
                await _registrationDataStore.AddItemAsync(registrationModel);
            }
            await App.LocalDatabase.DeleteAllRegistrationItemsAsync();
            return localRegistrations;
        }

        //private List<RegistrationValueModel> ConvertToRegistrationValues(List<RegistrationValueModel> registrationValues)
        //{
        //    var entities = new List<RegistrationValueModel>();
        //    foreach (var item in registrationValues)
        //    {
        //        var x = new RegistrationValueModel();
        //        x.Id = item.Id;
        //        x.RegistrationId = item.RegistrationId;
        //        x.SubItemId = item.SubItemId;
        //        x.SubItemName = item.SubItemName;
        //        x.Value = item.Value;

        //        entities.Add(x);
        //    }
        //    return entities;
        //}
    }
}
