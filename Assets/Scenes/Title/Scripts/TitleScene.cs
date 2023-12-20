using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class TitleScene : SceneBase
{
    [SerializeField]
    private Button _homeScene;

    [SerializeField]
    private GameObject _titlelogo;

    protected async override UniTask OnInitialize(object args)
    {
        await base.OnInitialize(args);
        Debug.Log("TitleScene OnInitialize");

        MainSystem.Instance.SoundManager.PlayBgm(ConstAddress.TitleBgm).Forget();

        _titlelogo.transform.DOScale(1.05f, 0.3f).SetLoops(-1, LoopType.Yoyo).WithCancellation(CancelToken).Forget();

        _homeScene.onClick.AddListener(() =>
        {
            MainSystem.Instance.SoundManager.PlaySe(ConstAddress.ButtonPush_0).Forget();

            _homeScene.gameObject.transform.DOPunchScale(
                punch: Vector3.one * 0.1f,
                duration: 0.2f,
                vibrato: 1
            )
            .SetEase(Ease.OutExpo)
            .OnComplete(() =>
            {
                MainSystem.Instance.AppSceneManager.ChangeScene(ConstSceneName.Home, fadeType: FadeType.ColorWhite);
            });
        });
    }
}