using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Common;
using Common.Commands;
using NSubstitute;
using Xunit;

namespace WPF.Tests
{
    public class MenuItemDeleterCommandTest
    {
        private MenuItemDeleterCommand _target;
        //private readonly IProcessor _processor = NSubstitute.Substitute.For<IProcessor>();

        public MenuItemDeleterCommandTest()
        {
            //_target = new MenuItemDeleterCommand(_processor);
        }

        [Theory]
        [InlineData(0xc0ffee)]
        [InlineData(1)]
        public async Task TestMethod(int menuItemId)
        {
            var menuItem = new MenuItemEntityModel{Id = menuItemId};

            await _target.ExecuteAsync(menuItem);
            
            //await _processor.Received().DeleteMenuItemAsync(Arg.Is(menuItemId));
        }

        [Fact]
        public async Task Raises_MenuItemDeleted()
        {
            var menuItem = new MenuItemEntityModel();
            var listener = Substitute.For<EventHandler<ExecuteAsyncCompletedEventArgs>>();
            _target.MenuItemDeleted += listener;

            await _target.ExecuteAsync(menuItem);

            listener.Received().Invoke(Arg.Is(_target), Arg.Any<ExecuteAsyncCompletedEventArgs>());
        }


        [Fact]
        public async Task Awaits_MenuItemDeleted_Async_Event_Handlers()
        {
            var menuItem = new MenuItemEntityModel();
            var expected = new Exception();

            _target.MenuItemDeleted += (s, e) => { e.AsyncEventHandlers.Add(((Func<Task>) (async () =>
            {
                await Task.Yield();
                throw expected;
            }))());};

            Assert.Same(expected, await Record.ExceptionAsync(() => _target.ExecuteAsync(menuItem)));
        }
    }
}
