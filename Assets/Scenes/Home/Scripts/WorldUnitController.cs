using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class WorldUnitController : MonoBehaviour
{
    private Queue<Vector3> _roots;
    private bool _isTweenMove;
    private CancellationToken _token;

    private void Start()
    {
        _token = this.GetCancellationTokenOnDestroy();
    }

    public async UniTaskVoid SetRoot(Queue<Vector3> roots)
    {
        _roots = roots;

        if (_isTweenMove)
        {
            return;
        }

        _isTweenMove = true;

        while (_roots.Count > 0)
        {
            Vector3 nextPosition = _roots.Dequeue();
            await MovePosition(nextPosition);
        }

        _isTweenMove = false;
    }

    private async UniTask MovePosition(Vector3 position)
    {
        if (Camera.main == null)
        {
            return;
        }

        await Camera.main.transform.DOMove(new Vector3(position.x + 1, position.y + 1, position.z + -10), 0.2f).WithCancellation(_token);
        await transform.DOMove(position, 0.3f).WithCancellation(_token);
    }
}
