using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class SceneBase : MonoBehaviour
{
    [Header("実行時このシーンから始める")]
    [SerializeField]
    private bool _isDebug;

    protected CancellationToken CancelToken;

    private void Awake()
    {
        if (!Debug.isDebugBuild)
        {
            _isDebug = false;
        }

        if (MainSystem.Instance == null)
        {
            if (_isDebug)
            {
                BootScene.NextSceneName = SceneManager.GetActiveScene().name;
            }

            SceneManager.LoadScene(ConstSceneName.Boot);
        }
    }

    protected virtual async UniTask OnInitialize(object args)
    {
        CancelToken = this.GetCancellationTokenOnDestroy();
        await UniTask.NextFrame();
    }

    protected virtual async UniTask OnFadeEndAction(object args)
    {
        await UniTask.NextFrame();
    }

    public async UniTask Initialize(object args)
    {
        await OnInitialize(args);
    }

    public async UniTask OnFadeEnd(object args)
    {
        await OnFadeEndAction(args);
    }
}