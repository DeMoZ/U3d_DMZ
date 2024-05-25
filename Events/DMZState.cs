using System;
using System.Collections.Generic;

namespace DMZ.Events
{
    public class DMZState<T> : IDisposable
    {
        private readonly List<Action<T>> _subscribers = new();
        protected readonly object _lock = new();
        protected T _data;

        public DMZState(T state = default)
        {
            _data = state;
        }
        
        public void Subscribe(Action<T> callback)
        {
            lock (_lock)
            {
                _subscribers.Add(callback);
            }
        }

        public void Unsubscribe(Action<T> callback)
        {
            lock (_lock)
            {
                _subscribers.Remove(callback);
            }
        }

        public T Value
        {
            get => _data;
            set
            {
                lock (_lock)
                {
                    if (Equals(value, _data))
                        return;

                    _data = value;
                }

                NotifySubscribers();
            }
        }

        public void SetValueAndForceNotify(T value)
        {
            lock (_lock)
            {
                _data = value;
            }

            NotifySubscribers();
        }

        protected virtual void NotifySubscribers()
        {
            List<Action<T>> subscribersCopy;

            lock (_lock)
            {
                subscribersCopy = new List<Action<T>>(_subscribers);
            }

            foreach (var subscriber in subscribersCopy)
            {
                subscriber?.Invoke(_data);
            }
        }

        public void Dispose()
        {
            lock (_lock)
            {
                _subscribers.Clear();
            }
        }
    }
}