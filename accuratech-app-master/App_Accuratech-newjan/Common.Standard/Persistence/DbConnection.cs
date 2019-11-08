using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Standard.Persistence
{
    public class DbConnection
    {
        private static readonly object syncRoot = new object();
        private static DbConnection instance;

        public static DbConnection Instance
        {
            get
            {
                if (instance != null)
                {
                    return instance;
                }

                lock (syncRoot)
                {
                    if (instance == null)
                    {
                        instance = new DbConnection();
                    }
                }

                return instance;
            }
        }

        public DatabaseContextOffline DbContextOffline => new DatabaseContextOffline();

        public DbConnection()
        {

        }
    }
}
