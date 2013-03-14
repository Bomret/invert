using System;
using System.Collections.Generic;
using System.Linq;
using Invert.Contracts;
using Invert.Types;

namespace Invert {
    public class PubSub {
        static PubSub _instance;
        readonly Dictionary<Type, List<HandlerWrapper>> _handlers;

        PubSub() {
            _handlers = new Dictionary<Type, List<HandlerWrapper>>();
        }

        public static PubSub Get() {
            return _instance ?? (_instance = new PubSub());
        }

        public void Subscribe<T>(IHandle<T> handler) {
            var interfaces = handler.GetType().GetInterfaces().Where(i => i.IsGenericType);

            foreach (var iFace in interfaces) {
                var type = iFace.GetGenericArguments()[0];
                var method = iFace.GetMethod("Handle");

                if (!_handlers.ContainsKey(type))
                    _handlers.Add(type, new List<HandlerWrapper>());

                var wrapper = new HandlerWrapper(new WeakReference(handler), method);

                _handlers[type].Add(wrapper);
            }
        }

        public void Unsubscribe<T>(IHandle<T> handler) {
            var interfaces = handler.GetType().GetInterfaces().Where(i => i.IsGenericType);

            foreach (var type in interfaces.Select(i => i.GetGenericArguments()[0])) {
                if (!_handlers.ContainsKey(type))
                    return;

                var found = _handlers[type].FirstOrDefault(w => w.Matches(handler));
                _handlers[type].Remove(found);
            }
        }

        public void Publish<T>(T message) {
            var type = message.GetType();
            var supportedHandlers = _handlers[type];

            foreach (var handler in supportedHandlers) {
                handler.Handle(message);
            }
        }
    }
}