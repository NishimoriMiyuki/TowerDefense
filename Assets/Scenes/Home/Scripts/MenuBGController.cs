using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

public class MenuBGController : MonoBehaviour
{
    [SerializeField]
    VorotaDoorController _doorController;

    [SerializeField]
    ValveController _valveController;

    [SerializeField]
    LeverController _leverController;

    [SerializeField]
    MenuBGCameraController _menuBgCameraController;

    [SerializeField]
    GameObject[] _lightObjects;

    private void Awake()
    {
        DisplayLightObjects(false);
    }

    public async UniTask OpenDoorAsync()
    {
        MainSystem.Instance.SoundManager.StopBgm();
        _valveController.StartValveAnimation();
        MainSystem.Instance.SoundManager.PlaySe(ConstAddress.DoorNobu).Forget();
        await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
        _doorController.OpenGridDoorAnimationAsync().Forget();
        await UniTask.Delay(TimeSpan.FromSeconds(VorotaDoorController.OpenGridAnimationTime));
        MainSystem.Instance.SoundManager.PlaySe(ConstAddress.Lever).Forget();
        await _leverController.StartLeverAnimation();
        DisplayLightObjects(true);
        _doorController.OpenMainDoorAnimationAsync().Forget();
        MainSystem.Instance.SoundManager.PlaySe(ConstAddress.Door).Forget();
        await UniTask.Delay(TimeSpan.FromSeconds(0.2f));
        _menuBgCameraController.Zoom();
        await UniTask.Delay(TimeSpan.FromSeconds(VorotaDoorController.OpenDoorAnimationTime));
    }

    public void Display(bool display)
    {
        this.gameObject.SetActive(display);
    }

    private void DisplayLightObjects(bool display)
    {
        foreach (var lightObj in _lightObjects)
        {
            lightObj.SetActive(display);
        }
    }
}
