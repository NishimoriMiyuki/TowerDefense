using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverPanel : MonoBehaviour
{
    [SerializeField]
    private Button _okButton;

    public void Init()
    {
        MainSystem.Instance.SoundManager.PlayBgm(ConstAddress.GameOver, isOneShot: true).Forget();

        _okButton.onClick.AddListener(OnClickOkButton);
    }

    private void OnClickOkButton()
    {
        MainSystem.Instance.SoundManager.PlaySe(ConstAddress.ButtonPush_0).Forget();

        MainSystem.Instance.AppSceneManager.ChangeScene(ConstSceneName.Home, fadeType: FadeType.Default);
    }
}
