using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public enum FadeType
{
    None,
    ColorWhite,
    ColorBlack,
    Default = ColorWhite,
}

public class FadeManager : MonoBehaviour
{
    [SerializeField]
    private FadePlayerBase _fadeplayerColor;

    private const float FADE_OUT_TIME = 0.5f;
    private const float FADE_IN_TIME = 0.5f;

    private CancellationToken _cancellationToken;

    private void Awake()
    {
        _cancellationToken = this.GetCancellationTokenOnDestroy();
    }

    public async UniTask FadeOut(FadeType fadeType)
    {
        if (fadeType == FadeType.None)
        {
            return;
        }

        _fadeplayerColor.SetColor(GetColor(fadeType));
        await _fadeplayerColor.FadeOut(FADE_OUT_TIME);
    }

    public async UniTask FadeIn(FadeType fadeType)
    {
        if (fadeType == FadeType.None)
        {
            return;
        }

        _fadeplayerColor.SetColor(GetColor(fadeType));
        await _fadeplayerColor.FadeIn(FADE_IN_TIME);
    }

    public void Hide()
    {
        _fadeplayerColor.Hide();
    }

    private Color GetColor(FadeType fadeType)
    {
        switch (fadeType)
        {
            case FadeType.ColorWhite:
                return Color.white;
            case FadeType.ColorBlack:
                return Color.black;
            default:
                return Color.white;
        }
    }
}
