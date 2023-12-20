using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class HomeUIManager : SingletonBehaviourSceneOnly<HomeUIManager>
{
    [SerializeField]
    private Canvas _mainMenuCanvas, _panelCanvas;

    [SerializeField]
    private Button _stageSelectButton, _unitOrganizationButton, _backButton, _powerUpButton;

    [SerializeField]
    private MenuBGController _menuBGController;

    [SerializeField]
    private Animation _animation;

    [SerializeField]
    private XpController _xpController;

    public void Init()
    {
        _mainMenuCanvas.gameObject.SetActive(true);
        _panelCanvas.gameObject.SetActive(true);
        _backButton.gameObject.SetActive(false);

        _stageSelectButton.onClick.AddListener(() =>
        {
            OnClickStageSelectButton().Forget();
        });

        _unitOrganizationButton.onClick.AddListener(() =>
        {
            OnClickUnitOrganizationButton();
        });

        _powerUpButton.onClick.AddListener(() =>
        {
            OnClickPowerUpButton();
        });

        _backButton.onClick.AddListener(OnClickBackButton);

        _xpController.Init();
    }

    private async UniTask OnClickStageSelectButton()
    {
        MainSystem.Instance.SoundManager.PlaySe(ConstAddress.ButtonPush_0).Forget();
        _animation.Play();
        await _menuBGController.OpenDoorAsync();
        OpenPanel(ConstAddress.StageSelectPanel).Forget();
    }

    private void OnClickUnitOrganizationButton()
    {
        MainSystem.Instance.SoundManager.PlaySe(ConstAddress.ButtonPush_0).Forget();
        _animation.Play();
        OpenPanel(ConstAddress.UnitOrganizationPanel, FadeType.ColorBlack).Forget();
    }

    private void OnClickPowerUpButton()
    {
        MainSystem.Instance.SoundManager.PlaySe(ConstAddress.ButtonPush_0).Forget();
        _animation.Play();
        OpenPanel(ConstAddress.PowerUpPanel, FadeType.ColorBlack).Forget();
    }

    private void OnClickBackButton()
    {
        MainSystem.Instance.SoundManager.PlaySe(ConstAddress.BackButton_0).Forget();

        MainSystem.Instance.AppSceneManager.ChangeScene(ConstSceneName.Home, fadeType: FadeType.Default);
    }

    public async UniTask OpenPanel(string panelAddress, FadeType fadeType = FadeType.Default, object args = null)
    {
        await MainSystem.Instance.FadeManager.FadeOut(fadeType);
        _menuBGController.Display(false);
        _backButton.gameObject.SetActive(true);
        var panel = await MainSystem.Instance.AddressableManager.InstantiateAsync(panelAddress, _panelCanvas.transform);

        if (!panel.TryGetComponent<PanelBase>(out var panelBase))
        {
            Debug.LogError("PanelにPanelBaseが付いていません");
            return;
        }

        await panelBase.Initialize(args);
        await MainSystem.Instance.FadeManager.FadeIn(fadeType);
    }

    public async UniTask ClosePanel(PanelBase panel, FadeType fadeType = FadeType.Default)
    {
        await MainSystem.Instance.FadeManager.FadeOut(fadeType);
        Destroy(panel.gameObject);
        _mainMenuCanvas.gameObject.SetActive(true);
        await MainSystem.Instance.FadeManager.FadeIn(fadeType);
    }

    public void XpRefresh()
    {
        _xpController.Refresh();
    }
}
