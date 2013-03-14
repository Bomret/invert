# invert

Invert is a small library that contains several classes to ease inversion of control.

## PubSub

The PubSub class provides methods to subscribe and unsubscribe message handlers and publish messages from a central place. This is especially useful in UI programming when several views want to exchange messages.

It is heavily inspired by the EventAggregator in Caliburn.Micro.

### Obtaining a reference

There is only one PubSub container available for an application which can be obtained by calling a simple static method.

```csharp
var pubSub = PubSub.Get();
```

### Subscribing a message handler

Classes that want to handle messages that are published via the PubSub class have to implement the ```IHandle``` interface and specify the message type they want to handle. They can implement the interface several times with different message types.

```csharp
class MessageHandler : IHandle<AbortMessage>, IHandle<LogOutEvent> {
	void Handle(AbortMessage message) {
		// handle the message
	}

	void Handle(LogOutEvent message) {
		// handle the message
	}
}
```

Subscribing is straight forward:

```csharp
var handler = new MessageHandler();

pubSub.Subscribe(handler);
```

### Unsubscribing a message handler

```csharp
pubsSub.Unsubscribe(handler);
```

### Publishing a message

To publish a message simply use the ```Publish``` message:

```csharp
var message = new LogInEvent("John", "john@somewhere.com");

pubSub.Publish(message);
```