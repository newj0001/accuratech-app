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
                _localDatabase.CreateTable<MenuItemEntity>();
                _localDatabase.CreateTable<SubItemEntity>();
                _localDatabase.CreateTable<RegistrationItemEntity>();
                _localDatabase.CreateTable<RegistrationValueItemEntity>();
            }
            catch (SQLiteException ex)
            {

                Debug.WriteLine(ex.Message);
            }
        }

        public List<RegistrationItemEntity> FetchRegistrationItems()
        {
            return _localDatabase.GetAllWithChildren<RegistrationItemEntity>().ToList();
        }

        public List<RegistrationValueItemEntity> FetchRegistrationValueItems()
        {
            return _localDatabase.GetAllWithChildren<RegistrationValueItemEntity>().ToList();
        }

        public Task SaveRegistrationItem(RegistrationItemEntity registrationItemEntity)
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

        public Task<List<MenuItemEntity>> LoadMenuItemsAsync()
        {
            var menuItems = _localDatabase.GetAllWithChildren<MenuItemEntity>();
            return Task.FromResult(menuItems);
        }

        public Task SaveMenuItemsAsync(ICollection<MenuItemEntity> items)
        {
            _localDatabase.InsertAll(items);
            ICollection<SubItemEntity> objects = items.SelectMany(i => i.SubItems).ToList();
            _localDatabase.InsertAll(objects);
            return Task.CompletedTask;
        }

        public Task DeleteAllRegistrationItemsAsync()
        {
            _localDatabase.DropTable<RegistrationItemEntity>();
            _localDatabase.DropTable<RegistrationValueItemEntity>();
            _localDatabase.CreateTables<RegistrationItemEntity, RegistrationValueItemEntity>();

            someEvent?.Invoke(FetchRegistrationItems().Count, FetchRegistrationValueItems().Count);

            return Task.CompletedTask;
        }

        public Task DeleteAllMenuItemAsync()
         {
            _localDatabase.DropTable<MenuItemEntity>();
            _localDatabase.DropTable<SubItemEntity>();

            _localDatabase.CreateTables<MenuItemEntity, SubItemEntity>();
            return Task.CompletedTask;
        }
    }
}
