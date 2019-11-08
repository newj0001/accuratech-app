using Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common_Backend.Context
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext() : base("Server=NTHVISION\\MSSQLSERVER1;Database=HoneyWellAppDB;Trusted_Connection=True;") { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<MenuItemEntityModel>().HasKey(mie => mie.Id);
            modelBuilder.Entity<SubItemEntityModel>().HasKey(sie => sie.Id);
            modelBuilder.Entity<RegistrationModel>().HasKey(r => r.Id);
            modelBuilder.Entity<RegistrationValueModel>().HasKey(rv => rv.Id);

            modelBuilder.Entity<MenuItemEntityModel>().HasMany(m => m.SubItems).WithRequired().HasForeignKey(s => s.MenuItemId).WillCascadeOnDelete(true);
            modelBuilder.Entity<RegistrationModel>().HasMany(r => r.RegistrationValues).WithRequired().HasForeignKey(rv => rv.RegistrationId).WillCascadeOnDelete(true);
            modelBuilder.Entity<MenuItemEntityModel>().HasMany(m => m.Registrations).WithRequired().HasForeignKey(r => r.MenuItemId).WillCascadeOnDelete(true);
            modelBuilder.Entity<RegistrationValueModel>().HasRequired(rv => rv.SubItemEntityModel).WithMany().HasForeignKey(rv => rv.SubItemId).WillCascadeOnDelete(false);

        }

        public DbSet<MenuItemEntityModel> Menus { get; set; }
        public DbSet<SubItemEntityModel> SubMenus { get; set; }
        public DbSet<RegistrationModel> Registrations { get; set; }
        public DbSet<RegistrationValueModel> RegistrationValues { get; set; }
    }
}
