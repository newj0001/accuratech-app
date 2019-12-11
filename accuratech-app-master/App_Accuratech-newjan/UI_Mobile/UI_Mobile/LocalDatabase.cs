using Common;
using SQLite;
using SQLiteNetExtensions.Extensions;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using UI_Mobile.Models;

namespace UI_Mobile
{
    public class LocalDatabase
    {
        readonly SQLiteConnection _localDatabase;

        public delegate void SomeEvent(int regCount, int recCount);
        public event SomeEvent someEvent;

        public LocalDatabase(string dbPath)
        {
            try
            {
                _localDatabase = new SQLiteConnection(dbPath);
                _localDatabase.CreateTable<MenuItemEntityModel>();
                _localDatabase.CreateTable<SubItemEntityModel>();
                _localDatabase.CreateTable<RegistrationModel>();
                _localDatabase.CreateTable<RegistrationValueModel>();
            }
            catch (SQLiteException ex)
            {

                Debug.WriteLine(ex.Message);
            }
        }

        public List<RegistrationModel> FetchRegistrationItems()
        {
            return _localDatabase.GetAllWithChildren<RegistrationModel>().ToList();
        }

        public List<RegistrationValueModel> FetchRegistrationValueItems()
        {
            return _localDatabase.GetAllWithChildren<RegistrationValueModel>().ToList();
        }

        public Task SaveRegistrationItem(RegistrationModel registrationItemEntity)
        {
            if (registrationItemEntity.Id != 0)
            {
                _localDatabase.UpdateWithChildren(registrationItemEntity);
            }
            else
            {
                _localDatabase.Insert(registrationItemEntity);

                foreach (var registrationValue in registrationItemEntity.RegistrationValues)
                {
                    registrationValue.RegistrationId = registrationItemEntity.Id;
                }

                _localDatabase.InsertAll(registrationItemEntity.RegistrationValues);
            }

            someEvent?.Invoke(FetchRegistrationItems().Count, FetchRegistrationValueItems().Count);

            return Task.CompletedTask;
        }

        public Task<List<MenuItemEntityModel>> LoadMenuItemsAsync()
        {
            var menuItems = _localDatabase.GetAllWithChildren<MenuItemEntityModel>();
            return Task.FromResult(menuItems);
        }

        public Task SaveMenuItemsAsync(List<MenuItemEntityModel> items)
        {
            _localDatabase.InsertAll(items);
            List<SubItemEntityModel> objects = items.SelectMany(i => i.SubItems).ToList();
            _localDatabase.InsertAll(objects);
            return Task.CompletedTask;
        }

        public Task DeleteAllRegistrationItemsAsync()
        {
            _localDatabase.DropTable<RegistrationModel>();
            _localDatabase.DropTable<RegistrationValueModel>();
            _localDatabase.CreateTables<RegistrationModel, RegistrationValueModel>();

            someEvent?.Invoke(FetchRegistrationItems().Count, FetchRegistrationValueItems().Count);

            return Task.CompletedTask;
        }

        public Task DeleteAllMenuItemAsync()
         {
            _localDatabase.DropTable<MenuItemEntityModel>();
            _localDatabase.DropTable<SubItemEntityModel>();

            _localDatabase.CreateTables<MenuItemEntityModel, SubItemEntityModel>();
            return Task.CompletedTask;
        }
    }
}
