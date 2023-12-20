using System;
using Cysharp.Threading.Tasks;
using Linqs.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UnitObject : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private Image _image;

    [SerializeField]
    private GameObject _checkmarkObj;

    [SerializeField]
    private LongPressTrigger _longPressTrigger;

    private PlayerUnitData _playerUnitData;
    public PlayerUnitData PlayerUnitData => _playerUnitData;

    private Action<UnitObject> _onSelectCallback;

    public async UniTask Init(PlayerUnitData playerUnitData, Action<UnitObject> onSelectCallback)
    {
        _image.sprite = await MainSystem.Instance.AddressableManager.LoadAssetAsync<Sprite>(playerUnitData.MasterAlly.icon_asset_address_square);
        _playerUnitData = playerUnitData;
        _onSelectCallback = onSelectCallback;
        _longPressTrigger.OnLongPressDown.AddListener(OnLongPressDown);

        CheckmarkSwitch(false);
    }

    private void OnLongPressDown()
    {
        MainSystem.Instance.DialogManager.Alert(
            "キャラクター情報\n" +
            "レベル :          " + _playerUnitData.lv + "\n" +
            "体力    :         " + _playerUnitData.Hp + "\n" +
            "攻撃力 :          " + _playerUnitData.Attack
            ).Forget();
    }

    public void CheckmarkSwitch(bool isActive)
    {
        _checkmarkObj.SetActive(isActive);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _onSelectCallback.Invoke(this);
    }
}
