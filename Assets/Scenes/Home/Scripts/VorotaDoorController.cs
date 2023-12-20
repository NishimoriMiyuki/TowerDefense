using Cysharp.Threading.Tasks;
using UnityEngine;
using DG.Tweening;

public class VorotaDoorController : MonoBehaviour
{
    [SerializeField]
    private GameObject _gridObj;

    [SerializeField]
    private GameObject _doorObj;

    public static readonly float OpenGridAnimationTime = 1f;
    public static readonly float OpenDoorAnimationTime = 1f;

    public async UniTask OpenGridDoorAnimationAsync()
    {
        await _gridObj.transform.DOLocalMoveY(2.4f, OpenGridAnimationTime);
    }

    public async UniTask OpenMainDoorAnimationAsync()
    {
        await _doorObj.transform.DOLocalMoveY(2.4f, OpenDoorAnimationTime);
    }
}
