using System;
using System.Reflection;

namespace Invert.Types {
    internal class HandlerWrapper {
        readonly MethodInfo _method;
        readonly WeakReference _reference;

        public HandlerWrapper(WeakReference reference, MethodInfo method) {
            _reference = reference;
            _method = method;
        }

        public void Handle(object message) {
            _method.Invoke(_reference.Target, new[] {message});
        }

        public bool Matches(object handler) {
            return _reference.Target == handler;
        }
    }
}