using Cysharp.Threading.Tasks;
using UnityEngine;

public class PanelBase : MonoBehaviour
{
    protected virtual async UniTask OnInitialize(object args)
    {
        await UniTask.NextFrame();
    }

    public async UniTask Initialize(object args)
    {
        await OnInitialize(args);
    }
}
