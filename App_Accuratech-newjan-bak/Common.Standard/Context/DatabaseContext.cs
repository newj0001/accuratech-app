using Common.Standard.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Standard.Context
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext() : this(new DbContextOptionsBuilder().UseSqlServer("Server=NTHVISION\\MSSQLSERVER1;Database=HoneyWellAppDB;Trusted_Connection=True;").Options)
        {

        }

        public DatabaseContext(DbContextOptions dbContextOptions) : base(dbContextOptions) { }


        protected override void OnConfiguring(DbContextOptionsBuilder options) => options.UseSqlServer("Server=NTHVISION\\MSSQLSERVER1;Database=HoneyWellAppDB;Trusted_Connection=True;");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<MenuItemEntityModel>().HasKey(mie => mie.Id);
            modelBuilder.Entity<SubItemEntityModel>().HasKey(sie => sie.Id);
            modelBuilder.Entity<RegistrationModel>().HasKey(r => r.Id);
            modelBuilder.Entity<RegistrationValueModel>().HasKey(rv => rv.Id);

            modelBuilder.Entity<MenuItemEntityModel>().HasMany(m => m.SubItems).WithOne().HasForeignKey(s => s.MenuItemId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<RegistrationModel>().HasMany(r => r.RegistrationValues).WithOne().HasForeignKey(rv => rv.RegistrationId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<MenuItemEntityModel>().HasMany(m => m.Registrations).WithOne().HasForeignKey(r => r.MenuItemId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<RegistrationValueModel>().HasOne(rv => rv.SubItemEntityModel).WithMany().HasForeignKey(rv => rv.SubItemId).OnDelete(DeleteBehavior.SetNull);

        }

        public DbSet<MenuItemEntityModel> Menus { get; set; }
        public DbSet<SubItemEntityModel> SubMenus { get; set; }
        public DbSet<RegistrationModel> Registrations { get; set; }
        public DbSet<RegistrationValueModel> RegistrationValues { get; set; }
    }
}
