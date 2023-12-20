using System.Threading;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AppSceneManager : MonoBehaviour
{
    private CancellationToken _cancellationToken;

    private bool _isLoad;

    private void Awake()
    {
        _cancellationToken = this.GetCancellationTokenOnDestroy();
    }

    public void ChangeScene(string sceneName, LoadSceneMode loadSceneMode = LoadSceneMode.Single, object args = null, FadeType fadeType = FadeType.None)
    {
        if (_isLoad)
        {
            return;
        }

        LoadScene(sceneName, loadSceneMode, args, fadeType).Forget();
    }

    public async UniTaskVoid UnloadScene(string sceneName)
    {
        await SceneManager.UnloadSceneAsync(sceneName).WithCancellation(_cancellationToken);
    }

    private async UniTask LoadScene(string sceneName, LoadSceneMode loadSceneMode, object args, FadeType fadeType)
    {
        _isLoad = true;

        await MainSystem.Instance.FadeManager.FadeOut(fadeType);

        // 今いるシーンと同じシーンが読み込まれた時
        bool isReloadScene = SceneManager.loadedSceneCount == 1 && SceneManager.GetActiveScene().name == sceneName;
        
        if (!isReloadScene && SceneManager.GetSceneByName(sceneName).IsValid())
        {
            await SceneManager.UnloadSceneAsync(sceneName).WithCancellation(_cancellationToken);
        }

        await SceneManager.LoadSceneAsync(sceneName, loadSceneMode).WithCancellation(_cancellationToken);

        await Resources.UnloadUnusedAssets();
        MainSystem.Instance.AddressableManager.ReleseAllAsset();
        System.GC.Collect();

        SceneBase nextSceneBase = await GetNextSceneBase(sceneName);

        if (nextSceneBase == null)
        {
            Debug.LogError("SceneBaseが存在しませんでした = " + sceneName);
            return;
        }

        _isLoad = false;

        await nextSceneBase.Initialize(args);
        await MainSystem.Instance.FadeManager.FadeIn(fadeType);
        await nextSceneBase.OnFadeEnd(args);
    }

    private async UniTask<SceneBase> GetNextSceneBase(string sceneName)
    {
        SceneBase nextSceneBase = null;

        await UniTask.DelayFrame(5);
        nextSceneBase = FindObjectsByType<SceneBase>(FindObjectsSortMode.None).FirstOrDefault(_ => _.GetType().Name == sceneName);

        return nextSceneBase;
    }
}