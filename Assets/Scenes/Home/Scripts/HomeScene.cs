using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class HomeScene : SceneBase
{
    [SerializeField]
    private HomeUIManager _homeUIManager;

    protected override async UniTask OnInitialize(object args)
    {
        await base.OnInitialize(args);

        MainSystem.Instance.SoundManager.PlayBgm(ConstAddress.HomeBgm).Forget();

        _homeUIManager.Init();
    }
}
