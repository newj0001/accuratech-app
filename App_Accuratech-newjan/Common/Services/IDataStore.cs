using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Common.Services
{
    public interface IDataStore<T>
    {
        Task<Uri> AddItemAsync(T item);
        Task<T> UpdateItemAsync(T item, int id);
        Task<HttpStatusCode> DeleteItemAsync(int id);
        Task<T> GetItemAsync(int id);
        Task<ICollection<T>> GetItemsAsync();
    }
}
