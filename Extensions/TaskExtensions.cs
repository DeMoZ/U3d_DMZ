using System;
using System.Threading.Tasks;

namespace DMZ.Extensions
{
    public static class TaskExtensions
    {
        public static bool IsSucceeded(this Task task)
            => task.IsCompleted && !task.IsFaulted && !task.IsCanceled;

        public static Task ContinueOnMainThread<T>(this Task<T> task, Action<Task<T>> action)
            => task.ContinueWith(action, TaskScheduler.FromCurrentSynchronizationContext());


        public static void Forget(this Task task, Action<Exception> errorHandler = null)
        {
            task.ContinueWith(t =>
            {
                if (t.IsFaulted && errorHandler != null)
                {
                    errorHandler(t.Exception);
                }
            }, TaskContinuationOptions.OnlyOnFaulted);
        }
    }
}