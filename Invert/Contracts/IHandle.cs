﻿namespace Invert.Contracts {
    public interface IHandle<in T> {
        void Handle(T message);
    }
}