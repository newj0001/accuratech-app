using Common.Standard.Persistence.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Standard.Persistence
{
    public interface IEntityFrameworkService
    {
        void Insert(QueueItem queueItem);
    }
}
