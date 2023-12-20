using Cysharp.Threading.Tasks;
using UnityEngine;

public abstract class FadePlayerBase : MonoBehaviour
{
    private void Awake()
    {
        Initialize();
    }

    protected abstract void Initialize();

    public abstract void SetColor(Color color);
    public virtual async UniTask FadeOut(float duration) { await UniTask.NextFrame(); }
    public virtual async UniTask FadeIn(float duration) { await UniTask.NextFrame(); }
    public abstract void Hide();
}