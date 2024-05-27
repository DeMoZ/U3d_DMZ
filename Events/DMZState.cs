using System;
using System.Collections.Generic;

namespace DMZ.Events
{
    public class DMZState<T> : IDisposable
    {
        private readonly List<Action<T>> _subscribers = new();
        protected readonly object _lock = new();
        protected T _data;
        protected T _previousData;

        public DMZState(T state = default)
        {
            _previousData = state;
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

        public T PreviousValue => _previousData;
        
        public T Value
        {
            get => _data;
            set
            {
                lock (_lock)
                {
                    if (Equals(value, _data))
                        return;

                    _previousData = _data;
                    _data = value;
                }

                NotifySubscribers();
            }
        }

        /// <summary>
        /// set value and force notify even if state has not changed
        /// </summary>
        /// <param name="value"></param>
        public void SetAndForceNotify(T value)
        {
            lock (_lock)
            {
                _previousData = _data;
                _data = value;
            }

            NotifySubscribers();
        }

        /// <summary>
        /// set value to both data and previous data
        /// </summary>
        /// <param name="value"></param>
        public void ResetValue(T value)
        {
            lock (_lock)
            {
                _previousData = value;
                _data = value;
            }

            NotifySubscribers();
        }
        
        /// <summary>
        /// set value to both data and previous data and force notify even if state has not changed
        /// </summary>
        /// <param name="value"></param>
        public void ResetAndForceNotify(T value)
        {
            lock (_lock)
            {
                _previousData = value;
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