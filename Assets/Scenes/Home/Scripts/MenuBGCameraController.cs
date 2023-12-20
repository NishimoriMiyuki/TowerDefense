using UnityEngine;
using DG.Tweening;

public class MenuBGCameraController : MonoBehaviour
{
    [SerializeField]
    Camera _menuBgCamera;

    public void Zoom()
    {
        var localPos = _menuBgCamera.transform.localPosition;
        _menuBgCamera.transform.DOLocalMove(new Vector3(localPos.x, 0.8f, -1.5f), 1.5f);
    }
}
