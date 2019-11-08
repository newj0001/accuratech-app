using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Common.Services
{
    public class FieldItemDataStore : IDataStore<SubItemEntityModel>
    {
        public static string urlMenuItem = "http://172.30.1.141:44333/api/menuitem/";
        public static string urlFieldItem = "http://172.30.1.141:44333/api/fielditem/";

        private readonly HttpClient _apiClient;

        public FieldItemDataStore()
        {
            _apiClient = ApiHelper.GetApiClient();
        }
        public async Task<Uri> AddItemAsync(ICollection<SubItemEntityModel> item)
        {
            HttpResponseMessage response = await _apiClient.PostAsJsonAsync(urlFieldItem, item);
            response.EnsureSuccessStatusCode();
            return response.Headers.Location;
        }

        public async Task<SubItemEntityModel> UpdateItemAsync(SubItemEntityModel item, int id)
        {
            using (HttpResponseMessage response = await _apiClient.PutAsJsonAsync(urlFieldItem + id, item))
            {
                response.EnsureSuccessStatusCode();

                item = await response.Content.ReadAsAsync<SubItemEntityModel>();
                return item;
            }
        }

        public async Task<HttpStatusCode> DeleteItemAsync(int id)
        {
            HttpResponseMessage response = await _apiClient.DeleteAsync(urlFieldItem + id);
            return response.StatusCode;
        }

        public Task<SubItemEntityModel> GetItemAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<ICollection<SubItemEntityModel>> GetItemsAsync()
        {
            using (HttpResponseMessage response = await _apiClient.GetAsync(urlMenuItem))
            {
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsAsync<List<SubItemEntityModel>>();
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task<Uri> AddItemAsync(SubItemEntityModel item)
        {
            HttpResponseMessage response = await _apiClient.PostAsJsonAsync(urlFieldItem, item);
            response.EnsureSuccessStatusCode();
            return response.Headers.Location;
        }
    }
}
