using Common;
using SQLite;
using SQLiteNetExtensions.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UI_Mobile.Models;

namespace UI_Mobile
{
    public class LocalDatabase
    {
        readonly SQLiteConnection _database;

        public LocalDatabase(string dbPath)
        {
            try
            {
                _database = new SQLiteConnection(dbPath);
                _database.CreateTable<MenuItemEntity>();
                _database.CreateTable<SubItemEntity>();
                _database.CreateTable<RegistrationItemEntity>();
                _database.CreateTable<RegistrationValueItemEntity>();
            }
            catch (SQLiteException ex)
            {

                Debug.WriteLine(ex.Message);
            }
        }

        public List<RegistrationItemEntity> FetchRegistrationItems()
        {
            return _database.GetAllWithChildren<RegistrationItemEntity>().ToList();
        }

        public Task SaveRegistrationItem(RegistrationItemEntity registrationItemEntity)
        {
            if (registrationItemEntity.Id != 0)
            {
                _database.UpdateWithChildren(registrationItemEntity);
            }
            else
            {
                _database.Insert(registrationItemEntity);

                foreach (var registrationValue in registrationItemEntity.RegistrationValues)
                {
                    registrationValue.RegistrationId = registrationItemEntity.Id;
                }

                _database.InsertAll(registrationItemEntity.RegistrationValues);
            }

            return Task.CompletedTask;
        }

        public Task<int> DeleteAllRegistrationsAsync()
        {
            return Task.FromResult(_database.DropTable<RegistrationItemEntity>());
        }

        public Task<List<MenuItemEntity>> LoadMenuItemsAsync()
        {
            var menuItems = _database.GetAllWithChildren<MenuItemEntity>();
            return Task.FromResult(menuItems);
        }

        public Task SaveMenuItemsAsync(ICollection<MenuItemEntity> items)
        {
            _database.InsertAll(items);
            ICollection<SubItemEntity> objects = items.SelectMany(i => i.SubItems).ToList();
            _database.InsertAll(objects);
            return Task.CompletedTask;
        }

        public Task DeleteAllRegistrationItemsAsync()
        {
            _database.DropTable<RegistrationItemEntity>();
            _database.DropTable<RegistrationValueItemEntity>();
            
            _database.CreateTables<RegistrationItemEntity, RegistrationValueItemEntity>();
            return Task.CompletedTask;
        }

        public Task DeleteAllMenuItemAsync()
         {
            _database.DropTable<MenuItemEntity>();
            _database.DropTable<SubItemEntity>();

            _database.CreateTables<MenuItemEntity, SubItemEntity>();
            return Task.CompletedTask;
        }
    }
}
