/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using DMZ.DebugSystem;
using DMZ.Extensions;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace DMZ.ContentDelivery
{
	public interface IAddressableDownloader
	{
		UniTask<T> DownloadAsync<T>(string name, Action<float> onProgress, CancellationToken cancellationToken);
		UniTask InitializeAsync();
		bool IsInAddressable(string name);
	}

	public class AddressableDownloader : IAddressableDownloader
	{
		private List<string> existingKeys;
		
		public async UniTask InitializeAsync()
		{
			await Addressables.InitializeAsync(true);
			await Addressables.UpdateCatalogs();
			existingKeys = Addressables.ResourceLocators
				.SelectMany(x => x.Keys
					.Select(y => y.ToString()))
				.ToList();
		}
		
		public async UniTask<T> DownloadAsync<T>(string name, Action<float> onProgress,
			CancellationToken cancellationToken)
		{
			var isLocationExists = await IsInAddressableAsync(name, cancellationToken);
			if (!isLocationExists)
				return default;

			return await DownloadAssetAsync<T>(name, onProgress, cancellationToken);
		}

		private async UniTask<bool> IsInAddressableAsync(string name, CancellationToken cancellationToken)
		{
			var handle = Addressables.LoadResourceLocationsAsync(name);
			try
			{
				await handle.ToUniTask(cancellationToken: cancellationToken);
				var isExists = handle.Status == AsyncOperationStatus.Succeeded && handle.Result.Count > 0;
				if (!isExists)
					DMZLogger.Warning($"{nameof(AddressableDownloader)} {name} Failed to check exists; handle.Status: {handle.Status}");
				
				Addressables.Release(handle);
				return isExists;
			}
			catch (OperationCanceledException)
			{
				if (!handle.IsDone)
				{
					Addressables.Release(handle);
					DMZLogger.Log($"{nameof(AddressableDownloader)} {name} download was cancelled.");
				}
			}
			
			DMZLogger.Error($"{nameof(AddressableDownloader)} Failed to load {name}; handle.Status: {handle.Status}; {handle.OperationException}");
			return default;
		}
		
		public bool IsInAddressable(string name)
		{
			return existingKeys.Any(x => x.Same(name));
		}

		private async UniTask<T> DownloadAssetAsync<T>(string name, Action<float> onProgress,
			CancellationToken cancellationToken)
		{
			T result = default;
			var handle = Addressables.LoadAssetAsync<T>(name);
			try
			{
				await handle.ToUniTask(cancellationToken: cancellationToken);
				result = handle.Result;
				return result;
			}
			catch (OperationCanceledException)
			{
				if (!handle.IsDone)
				{
					Addressables.Release(handle);
					DMZLogger.Log($"{nameof(AddressableDownloader)} {name} download was cancelled.");
				}
			}

			DMZLogger.Error($"{nameof(AddressableDownloader)} Failed to load {name}; handle.Status: {handle.Status}; {handle.OperationException}");
			return default;
		}
	}
}*/