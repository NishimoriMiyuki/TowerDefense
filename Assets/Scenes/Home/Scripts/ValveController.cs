using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ValveController : MonoBehaviour
{
    [SerializeField]
    private GameObject _valveObj;

    public void StartValveAnimation()
    {
        var localRotate = _valveObj.transform.localRotation;
        _valveObj.transform.DOLocalRotate(new Vector3(localRotate.x, localRotate.y, localRotate.z + 360f), 3f, mode: RotateMode.FastBeyond360);
    }
}
