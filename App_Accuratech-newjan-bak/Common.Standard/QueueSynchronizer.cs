using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    class QueueSynchronizer
    {
        //public async Task Synchronize()
        //{
        //    var registrationDataStore = new RegistrationDataStore();
        //    var dbContext = new DatabaseContextOffline();
        //    foreach (var queue in dbContext.Queue.ToList())
        //    {
        //        var success = await registrationDataStore.AddItemAsync2(queue);
        //        if (success)
        //        {
        //            dbContext.Queue.Remove(queue);
        //            dbContext.SaveChanges();
        //        }
        //    }
        //}
    }
}
