using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class PowerUpPanel : PanelBase
{
    [SerializeField]
    private UnitLevelUpView _unitLevelUpView;

    [SerializeField]
    private UnitUnlockView _unitUnlockView;

    [SerializeField]
    private Button _levelUpViewButton, _unlockViewButton;

    protected async override UniTask OnInitialize(object args)
    {
        await base.OnInitialize(args);

        await _unitLevelUpView.Init(XpRefresh);
        await _unitUnlockView.Init(XpRefresh);

        LevelUpViewActive();

        _levelUpViewButton.onClick.AddListener(OnClickLevelUpViewButton);
        _unlockViewButton.onClick.AddListener(OnClickUnitUnlockViewButton);
    }

    private void OnClickLevelUpViewButton()
    {
        MainSystem.Instance.SoundManager.PlaySe(ConstAddress.ButtonPush_0).Forget();
        LevelUpViewActive();
    }

    private void OnClickUnitUnlockViewButton()
    {
        MainSystem.Instance.SoundManager.PlaySe(ConstAddress.ButtonPush_0).Forget();
        UnitUnlockViewActive();
    }

    private void LevelUpViewActive()
    {
        _unitLevelUpView.gameObject.SetActive(true);
        _unitUnlockView.gameObject.SetActive(false);
    }

    private void UnitUnlockViewActive()
    {
        _unitUnlockView.gameObject.SetActive(true);
        _unitLevelUpView.gameObject.SetActive(false);
    }

    private void XpRefresh()
    {
        HomeUIManager.Instance.XpRefresh();

        _unitLevelUpView.Refresh();
        _unitUnlockView.Refresh();
    }
}
