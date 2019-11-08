using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Common.Services;

namespace Common.Commands
{
    public class MenuItemDeleterCommand : ICommand
    {
        private readonly IDataStore<MenuItemEntityModel> _dataStore;

        public MenuItemDeleterCommand(IDataStore<MenuItemEntityModel> dataStore)
        {
            _dataStore = dataStore;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public async void Execute(object parameter)
        {
            await ExecuteAsync(parameter);
        }

        public async Task ExecuteAsync(object parameter)
        {
            await _dataStore.DeleteItemAsync(((MenuItemEntityModel)parameter).Id);
            var executeAsyncCompletedEventArgs = new ExecuteAsyncCompletedEventArgs();
            MenuItemDeleted?.Invoke(this, executeAsyncCompletedEventArgs);
            await Task.WhenAll(executeAsyncCompletedEventArgs.AsyncEventHandlers);
        }

        public event EventHandler CanExecuteChanged;
        public event EventHandler<ExecuteAsyncCompletedEventArgs> MenuItemDeleted;
    }

    public class ExecuteAsyncCompletedEventArgs : EventArgs
    {
        public ICollection<Task> AsyncEventHandlers { get; } = new List<Task>();
    }
}