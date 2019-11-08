using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Common.Commands;
using NSubstitute;
using Xunit;

namespace WPF.Tests
{
    public class FieldItemDeleterCommandTest
    {
        private FieldItemDeleterCommand _target;
        //private readonly IProcessor _processor = NSubstitute.Substitute.For<IProcessor>();

        public FieldItemDeleterCommandTest()
        {
            //_target = new FieldItemDeleterCommand(_processor);
        }

        [Theory]
        [InlineData(0xc0ffee)]
        [InlineData(1)]
        public async Task TestMethod(int fieldItemId)
        {
            var fieldItem = new SubItemEntityModel {Id = fieldItemId};

            await _target.ExecuteAsync(fieldItem);

            //await _processor.Received().DeleteFieldItemAsync(Arg.Is(fieldItemId));
        }

        [Fact]
        public async Task Raises_FieldItemDeleted()
        {
            var fieldItem = new SubItemEntityModel();
            var listener = Substitute.For<EventHandler<ExecuteAsyncCompletedEventArgs>>();
            _target.FieldItemDeleted += listener;

            await _target.ExecuteAsync(fieldItem);

            listener.Received().Invoke(Arg.Is(_target), Arg.Any<ExecuteAsyncCompletedEventArgs>());
        }

        [Fact]
        public async Task Awaits_FieldItemDeleted_Async_Event_Handlers()
        {
            var fieldItem = new SubItemEntityModel();
            var expected = new Exception();

            _target.FieldItemDeleted += (s, e) =>
            {
                e.AsyncEventHandlers.Add(((Func<Task>) (async () =>
                {
                    await Task.Yield();
                    throw expected;
                }))());
            };

            Assert.Same(expected, await Record.ExceptionAsync(() => _target.ExecuteAsync(fieldItem)));
        }
    }
}
