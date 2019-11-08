using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Common.Services
{
    //public class RegistrationValueDataStore : IDataStore<RegistrationValueModel>
    //{
    //    public static string urlRegistrationValues = "http://172.30.1.141:44333/api/registrationvalues/";

    //    private readonly HttpClient _apiClient;
    //    public RegistrationValueDataStore()
    //    {
    //        _apiClient = ApiHelper.GetApiClient();
    //    }
    //    public async Task<Uri> AddItemAsync(ICollection<RegistrationValueModel> value)
    //    {
    //        HttpResponseMessage response = await _apiClient.PostAsJsonAsync(urlRegistrationValues, value);
    //        response.EnsureSuccessStatusCode();
    //        return response.Headers.Location;
    //    }

    //    public Task<RegistrationValueModel> UpdateItemAsync(RegistrationValueModel item, int id)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public Task<HttpStatusCode> DeleteItemAsync(int id)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public Task<RegistrationValueModel> GetItemAsync(int id)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public Task<ICollection<RegistrationValueModel>> GetItemsAsync()
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public async Task<Uri> AddItemAsync(RegistrationValueModel value)
    //    {
    //        HttpResponseMessage response = await _apiClient.PostAsJsonAsync(urlRegistrationValues, value);
    //        response.EnsureSuccessStatusCode();
    //        return response.Headers.Location;
    //    }
    //}
}
