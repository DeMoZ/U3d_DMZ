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
	}
}