using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Common.Services;

namespace Common.Commands
{
    public class FieldItemDeleterCommand : ICommand
    {
        private readonly IDataStore<SubItemEntityModel> _dataStore;

        public FieldItemDeleterCommand(IDataStore<SubItemEntityModel> dataStore)
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
            await _dataStore.DeleteItemAsync(((SubItemEntityModel)parameter).Id);
            var executeAsyncCompletedEventArgs = new ExecuteAsyncCompletedEventArgs();
            FieldItemDeleted?.Invoke(this, executeAsyncCompletedEventArgs);
            await Task.WhenAll(executeAsyncCompletedEventArgs.AsyncEventHandlers);
        }

        public event EventHandler CanExecuteChanged;
        public event EventHandler<ExecuteAsyncCompletedEventArgs> FieldItemDeleted;
    }
}
