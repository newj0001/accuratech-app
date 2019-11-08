using Common.Standard.Persistence.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Common.Standard.Persistence
{
    public class EntityFrameworkService : IEntityFrameworkService
    {
        private DatabaseContextOffline _databaseContext;
        public void Insert(QueueItem queueItem)
        {
            var item = _databaseContext.QueueItems.ToList();

            if (item == null)
            {
                _databaseContext.QueueItems.Add(queueItem);
            }
            else
            {
                //item = 
            }
        }
    }
}
