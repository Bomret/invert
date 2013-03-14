using Invert;
using Invert.Contracts;
using Machine.Specifications;

namespace Invert_Specs {
    internal class When_I_publish_a_message_to_a_subscribed_handler {
        static PubSub _sut;
        static TestHandler _handler;
        static TestEvent _event;

        Establish ctx = () => {
            _sut = PubSub.Get();
            _handler = new TestHandler();
            _event = new TestEvent();

            _sut.Subscribe(_handler);
        };

        Because of = () => _sut.Publish(_event);

        It should_publish_the_message = () => _handler.Result.ShouldEqual(_event);

        internal class TestEvent {}

        class TestHandler : IHandle<TestEvent> {
            public TestEvent Result { get; private set; }

            public void Handle(TestEvent message) {
                Result = message;
            }
        }
    }

    internal class When_I_unsubscribe_a_subscribed_handler {
        static PubSub _sut;
        static TestHandler _handler;

        Establish ctx = () => {
            _sut = PubSub.Get();
            _handler = new TestHandler();

            _sut.Subscribe(_handler);
            _sut.Unsubscribe(_handler);
        };

        Because of = () => _sut.Publish(new TestEvent());

        It should_not_publish_messages_to_the_handler_anymore = () => _handler.Result.ShouldBeNull();

        internal class TestEvent {}

        class TestHandler : IHandle<TestEvent> {
            public TestEvent Result { get; private set; }

            public void Handle(TestEvent message) {
                Result = message;
            }
        }
    }
}