using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Common;
using Common.Model;
using WPF.Contracts;

namespace WPF.Repositories
{
    public class RegistrationDataStore : IDataStore<RegistrationModel>
    {
        public static string urlRegistration = "http://172.30.1.141:44333/api/registration/";

        private readonly HttpClient _apiClient;

        public RegistrationDataStore()
        {
            _apiClient = ApiHelper.GetApiClient();
        }

        public async Task<bool> AddItemAsync2(QueueEntity queue)
        {
            var registration = JsonConvert.DeserializeObject<RegistrationModel>(queue.Body);
            try
            {
                await AddItemAsync(registration, queue.Url);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<Uri> AddItemAsync(RegistrationModel item)
        {
            return await AddItemAsync(item, urlRegistration);
        }

        public async Task<Uri> AddItemAsync(RegistrationModel item, string url)
        {
            HttpResponseMessage response = await _apiClient.PostAsJsonAsync(urlRegistration, item);
            response.EnsureSuccessStatusCode();
            return response.Headers.Location;
        }

        public Task<RegistrationModel> UpdateItemAsync(RegistrationModel item, int id)
        {
            throw new NotImplementedException();
        }

        public Task<HttpStatusCode> DeleteItemAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<RegistrationModel> GetItemAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<ICollection<RegistrationModel>> GetItemsAsync()
        {
            using (HttpResponseMessage response = await _apiClient.GetAsync(urlRegistration))
            {
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsAsync<List<RegistrationModel>>();
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task<ICollection<RegistrationModel>> GetItemsAsync(int menuItemId)
        {
            using (HttpResponseMessage response = await _apiClient.GetAsync(urlRegistration + $"{menuItemId}"))
            {
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsAsync<List<RegistrationModel>>();
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }
    }
}
