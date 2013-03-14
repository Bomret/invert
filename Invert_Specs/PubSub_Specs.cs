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

    internal class When_I_publish_a_message_to_a_subscribed_handler_that_handles_several_messages {
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

        It should_leave_the_message_at_null = () => _handler.Message.ShouldBeNull();

        It should_publish_the_event = () => _handler.Event.ShouldEqual(_event);

        internal class TestEvent {}

        class TestHandler : IHandle<TestEvent>, IHandle<TestMessage> {
            public TestEvent Event { get; private set; }
            public TestMessage Message { get; private set; }

            public void Handle(TestEvent message) {
                Event = message;
            }

            public void Handle(TestMessage message) {
                Message = message;
            }
        }

        internal class TestMessage {}
    }

    internal class When_I_subscribe_a_class_that_does_not_implement_the_IHandle_interface_and_publish_a_message {
        static PubSub _sut;
        static TestHandler _handler;
        static TestMessage _message;

        Establish ctx = () => {
            _sut = PubSub.Get();
            _handler = new TestHandler();
            _message = new TestMessage();

            _sut.Subscribe(_handler);
        };

        Because of = () => _sut.Publish(_message);

        It should_leave_the_message_at_null = () => _handler.Message.ShouldBeNull();

        class TestHandler {
            public TestMessage Message { get; private set; }

            public void Handle(TestMessage message) {
                Message = message;
            }
        }

        internal class TestMessage {}
    }

    internal class
        When_I_subscribe_a_class_that_does_not_implement_the_IHandle_interface_but_others_and_publish_a_message {
        static PubSub _sut;
        static TestHandler _handler;
        static TestMessage _message;

        Establish ctx = () => {
            _sut = PubSub.Get();
            _handler = new TestHandler();
            _message = new TestMessage();

            _sut.Subscribe(_handler);
        };

        Because of = () => _sut.Publish(_message);

        It should_leave_the_message_at_null = () => _handler.Message.ShouldBeNull();

        class TestHandler : IDoNothing, IDoSomething<TestMessage> {
            public TestMessage Message { get; private set; }

            public void Do(TestMessage message) {
                Message = message;
            }
        }

        internal class TestMessage {}
    }

    internal interface IDoSomething<in T> {
        void Do(T message);
    }

    internal interface IDoNothing {}
}