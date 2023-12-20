using System;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitUnlockCell : MonoBehaviour
{
    [SerializeField]
    private Image _image;

    [SerializeField]
    private Button _button;

    [SerializeField]
    private TextMeshProUGUI _xpText;

    private Ally _allyData;
    private Action _funcOk;

    public async UniTask Init(Ally allyData, Action funcOK)
    {
        _image.sprite = await MainSystem.Instance.AddressableManager.LoadAssetAsync<Sprite>(allyData.icon_asset_address_square);
        _xpText.text = allyData.unlock_xp.ToString();

        _allyData = allyData;
        _funcOk = funcOK;

        _button.onClick.AddListener(OnClick);

        Refresh();
    }

    private void Refresh()
    {
        ButtonInteractable(false);

        if (MainSystem.Instance.PlayerData.xp < _allyData.unlock_xp)
        {
            return;
        }

        ButtonInteractable(true);
    }

    private void OnClick()
    {
        MainSystem.Instance.SoundManager.PlaySe(ConstAddress.ButtonPush_1).Forget();
        MainSystem.Instance.DialogManager.Confirm("キャラを開放しますか？", FunkOk).Forget();
    }

    private void ButtonInteractable(bool isActive)
    {
        _button.interactable = isActive;
    }

    private void FunkOk()
    {
        MainSystem.Instance.SoundManager.PlaySe(ConstAddress.ButtonPush_2).Forget();

        MainSystem.Instance.PlayerData.ReducedXp(_allyData.unlock_xp);
        MainSystem.Instance.PlayerData.unit.Add(new PlayerUnitData { ally_id = _allyData.id, lv = 1 });

        _funcOk.Invoke();
    }
}
