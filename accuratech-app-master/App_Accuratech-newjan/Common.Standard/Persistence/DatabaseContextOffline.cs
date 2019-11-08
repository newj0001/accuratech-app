using Common.Standard.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;
using System.IO;

namespace Common.Standard.Persistence
{
    public class DatabaseContextOffline : DbContext
    {
        public DbSet<QueueItem> QueueItems { get; set; }

        private static bool _created = false;
        private readonly string dbPath;
        private const string SqlLiteFilename = "queueDb.db";

        public DatabaseContextOffline()
        {
            if (!_created)
            {
                _created = true;
                Database.EnsureCreated();
            }
            dbPath = GetDatabasePath();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            try
            {
                base.OnConfiguring(optionsBuilder);
                optionsBuilder.UseSqlite($"Data Source={dbPath}");
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.InnerException.Message);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        private string GetDatabasePath()
        {
            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var path = Path.Combine(documentsPath, SqlLiteFilename);
            return path;
        }
    }
}
