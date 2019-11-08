using Common.Standard.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Standard.Context
{
    public class DatabaseContextOffline : DbContext
    {
        public DbSet<QueueEntityModel> QueueItems { get; set; }

        private string DatabasePath { get; set; }

        public DatabaseContextOffline()
        {
            
        }

        public DatabaseContextOffline(string databasePath)
        {
            DatabasePath = databasePath;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Filename={DatabasePath}");
        }
    }
}
