using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AddressableManager : MonoBehaviour
{
    /// <summary>
    /// ロードしてるアセットの情報
    /// </summary>
    public class LoadingAsset
    {
        // 参照カウンター
        public int ReferenceCounter = 0;

        public AsyncOperationHandle Handle = default;
    }

    private readonly Dictionary<string, LoadingAsset> _loadingAssetTable = new();

    /// <summary>
    /// アセット読み込む
    /// 使い終わったらReleaseAssetを呼ぶ必要がある
    /// </summary>
    public void LoadAsset<T>(string address, UnityAction<T> onCompleted = null, bool releaseImmediately = false) where T : UnityEngine.Object
    {
        Debug.Log($"AddressableManager.LoadAsset address={address}");
        Action<AsyncOperationHandle> completeAction = (x) =>
        {
            onCompleted?.Invoke((T)x.Result);

            if (releaseImmediately)
            {
                ReleaseAsset(address);
            }
        };

        // ロード済み
        if (_loadingAssetTable.TryGetValue(address, out LoadingAsset x))
        {
            x.ReferenceCounter++;

            if (x.Handle.IsDone)
            {
                completeAction?.Invoke(x.Handle);
            }
            else
            {
                x.Handle.Completed += completeAction;
            }
        }
        // ロードする
        else
        {
            AsyncOperationHandle handle = Addressables.LoadAssetAsync<T>(address);
            handle.Completed += completeAction;

            _loadingAssetTable.Add(address, new LoadingAsset() { ReferenceCounter = 1, Handle = handle });
        }
    }

    public async UniTask<T> LoadAssetAsync<T>(string address, CancellationToken cancellationToken = default) where T : UnityEngine.Object
    {
        Debug.Log($"AddressableManager.LoadAssetAsync address={address}");
        if (cancellationToken.IsCancellationRequested)
        {
            throw new OperationCanceledException();
        }
        if (_loadingAssetTable.TryGetValue(address, out LoadingAsset asset))
        {
            ++asset.ReferenceCounter;

            if (asset.Handle.IsDone)
            {
                return asset.Handle.Result as T;
            }
            else
            {
                await asset.Handle.Task;
                return (T)asset.Handle.Result;
            }
        }
        else
        {
            AsyncOperationHandle handle = Addressables.LoadAssetAsync<T>(address);
            _loadingAssetTable.Add(address, new LoadingAsset() { ReferenceCounter = 1, Handle = handle });

            await handle.WithCancellation(cancellationToken);
            return (T)handle.Result;
        }
    }

    public async UniTask<IList<T>> LoadAssetsAsync<T>(string key, CancellationToken cancellationToken = default)
    {
        Debug.Log($"AddressableManager.LoadAssetAsync address={key}");
        if (_loadingAssetTable.TryGetValue(key, out var loadingAsset))
        {
            return loadingAsset.Handle.Result as IList<T>;
        }

        var handle = Addressables.LoadAssetsAsync<T>(key, null);
        await handle.WithCancellation(cancellationToken);
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            _loadingAssetTable[key] = new LoadingAsset { ReferenceCounter = 1, Handle = handle };
        }
        return handle.Result;
    }

    public void ReleaseAsset(string address)
    {
        if (_loadingAssetTable.TryGetValue(address, out LoadingAsset value))
        {
            value.ReferenceCounter--;
            if (value.ReferenceCounter <= 0)
            {
                Debug.Log($"Unload Asset: {address}");
                Addressables.Release(value.Handle);

                _loadingAssetTable.Remove(address);
            }
        }
        else
        {
            Debug.LogWarning(address + "は読み込まれていません");
        }
    }

    public void ReleseAllAsset()
    {
        foreach (var (key, loadingAsset) in _loadingAssetTable)
        {
            Debug.Log($"Unload Asset: {key}");
            Addressables.Release(loadingAsset.Handle);
        }
        _loadingAssetTable.Clear();
    }

    public void Instantiate(string address, UnityAction<GameObject> onCompleted = null, bool autoRelease = true)
    {
        Debug.Log($"AddressableManager.LoadAssetAsync address={address}");
        AsyncOperationHandle<GameObject> handle = Addressables.InstantiateAsync(address);
        handle.Completed += (x) =>
        {
            var instance = x.Result;

            if (autoRelease)
            {
                instance.AddComponent<AddressableInstanceAutoRelease>();
            }

            onCompleted?.Invoke(instance);
        };
    }

    public async UniTask<GameObject> InstantiateAsync(string address, Transform parent = null, bool autRelease = true, CancellationToken cancellationToken = default)
    {
        Debug.Log($"AddressableManager.LoadAssetAsync address={address}");
        var instance = await Addressables.InstantiateAsync(address, parent).WithCancellation(cancellationToken);
        if (autRelease)
        {
            instance.AddComponent<AddressableInstanceAutoRelease>();
        }
        return instance;
    }

    public void ReleaseInstance(GameObject instance)
    {
        Debug.Log($"Unload Asset: {instance}");
        Addressables.ReleaseInstance(instance);
    }
}