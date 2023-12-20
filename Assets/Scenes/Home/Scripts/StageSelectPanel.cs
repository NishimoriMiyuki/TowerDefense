using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class StageSelectPanel : PanelBase
{
    [SerializeField]
    private Button _gameSceneButton;

    [SerializeField]
    private StageSelectScrollView _scrollView;

    private List<WorldStage> _worldStageData = new();

    protected override async UniTask OnInitialize(object args)
    {
        await base.OnInitialize(args);

        MainSystem.Instance.SoundManager.PlayBgm(ConstAddress.StageSelectBgm).Forget();

        _gameSceneButton.onClick.AddListener(OnClickGameSceneButton);

        _worldStageData = MainSystem.Instance.Master.WorldStageData;
        await _scrollView.Init(_worldStageData);
    }

    private void OnClickGameSceneButton()
    {
        if (_scrollView.IsDragging)
        {
            return;
        }

        MainSystem.Instance.SoundManager.PlaySe(ConstAddress.ButtonPush_0).Forget();

        MainSystem.Instance.AppSceneManager.ChangeScene(
            ConstSceneName.Game,
            fadeType: FadeType.ColorBlack,
            args: _scrollView.CurrenSelectedWorldStage);
    }
}
