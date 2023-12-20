using System;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitLvUpCell : MonoBehaviour
{
    [SerializeField]
    private Image _image;

    [SerializeField]
    private Button _button;

    [SerializeField]
    private TextMeshProUGUI _lvText, _needText;

    [SerializeField]
    private GameObject _requirdXpTexts;

    private PlayerUnitData _playerUnitData;
    private Action _funkOk;

    public async UniTask Init(PlayerUnitData playerUnitData, Action funkOk)
    {
        _image.sprite = await MainSystem.Instance.AddressableManager.LoadAssetAsync<Sprite>(playerUnitData.MasterAlly.icon_asset_address_square);
        _funkOk = funkOk;

        _button.onClick.AddListener(OnClick);

        _playerUnitData = playerUnitData;

        Refresh();
    }

    private void Refresh()
    {
        ButtonInteractable(false);
        _requirdXpTexts.SetActive(false);
        _lvText.text = _playerUnitData.lv + "Lv";

        if (_playerUnitData.MasterPowerup == null)
        {
            Debug.Log("powerupData null");
            _lvText.text = "MaxLv ";
            _needText.text = "";
            return;
        }

        _needText.text = _playerUnitData.MasterPowerup.requird_xp.ToString();
        _requirdXpTexts.SetActive(true);

        if (MainSystem.Instance.PlayerData.xp < _playerUnitData.MasterPowerup.requird_xp)
        {
            return;
        }

        ButtonInteractable(true);
    }

    private void OnClick()
    {
        if (_playerUnitData.MasterPowerup == null)
        {
            return;
        }

        MainSystem.Instance.SoundManager.PlaySe(ConstAddress.ButtonPush_1).Forget();

        MainSystem.Instance.DialogManager.Confirm("レベルアップしますか？\n" +
            _playerUnitData.lv + "レベル　→　" + _playerUnitData.NextLv + "レベル\n" +
            "体力 " + _playerUnitData.Hp + "　→　" + _playerUnitData.NextLvHp + "\n" +
            "攻撃力 " + _playerUnitData.Attack + "　→　" + _playerUnitData.NextLvAttack,
            FunkOk).Forget();
    }

    private void FunkOk()
    {
        MainSystem.Instance.SoundManager.PlaySe(ConstAddress.ButtonPush_2).Forget();

        MainSystem.Instance.PlayerData.ReducedXp(_playerUnitData.MasterPowerup.requird_xp);
        _playerUnitData.LvUp();

        _funkOk.Invoke();
    }

    private void ButtonInteractable(bool isActive)
    {
        _button.interactable = isActive;
    }
}
