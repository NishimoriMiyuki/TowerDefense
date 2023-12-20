using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClearPanel : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _rewardXpText;

    [SerializeField]
    private Button _okButton;

    public void Init(int rewardXp)
    {
        MainSystem.Instance.SoundManager.PlayBgm(ConstAddress.Victory,isOneShot: true).Forget();

        _rewardXpText.text = rewardXp.ToString();

        _okButton.onClick.AddListener(OnClickOkButton);
    }

    private void OnClickOkButton()
    {
        MainSystem.Instance.SoundManager.PlaySe(ConstAddress.ButtonPush_0).Forget();

        MainSystem.Instance.AppSceneManager.ChangeScene(ConstSceneName.Home, fadeType: FadeType.Default);
    }
}
