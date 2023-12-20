using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;

public class LeverController : MonoBehaviour
{
    [SerializeField]
    private GameObject _leverObj;

    private const float LEVER_ANIMATION_TIME = 0.5f;

    public async UniTask StartLeverAnimation()
    {
        var localRotate = _leverObj.transform.localRotation;
        await _leverObj.transform.DOLocalRotate(new Vector3(180f, localRotate.y, localRotate.z), LEVER_ANIMATION_TIME, mode: RotateMode.LocalAxisAdd);
    }
}
