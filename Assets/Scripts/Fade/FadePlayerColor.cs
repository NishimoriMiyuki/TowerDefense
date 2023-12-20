using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FadePlayerColor : FadePlayerBase
{
    [SerializeField]
    private Image _image;

    private Color _color = Color.white;

    protected override void Initialize()
    {
        _image.enabled = false;
    }

    public override void SetColor(Color color)
    {
        _color = color;
    }

    public override async UniTask FadeOut(float duration)
    {
        _image.enabled = true;
        Color color = _color;
        color.a = 0f;
        _image.color = color;
        await _image.DOFade(1f, duration);
    }

    public override async UniTask FadeIn(float duration)
    {
        Color color = _color;
        color.a = 1f;
        _image.color = color;
        await _image.DOFade(0f, duration);
        _image.enabled = false;
    }

    public override void Hide()
    {
        _image.enabled = false;
    }
}