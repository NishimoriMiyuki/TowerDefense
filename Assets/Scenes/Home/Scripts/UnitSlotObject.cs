using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class UnitSlotObject : MonoBehaviour
{
    [SerializeField]
    private Button _button;

    [SerializeField]
    private Image _image;

    [SerializeField]
    private GameObject _checkmarkObj;

    private int _slotNumber;
    public int SlotNumber => _slotNumber;

    public void Init(int slotNumber, Action<UnitSlotObject> onSelectCallback)
    {
        _slotNumber = slotNumber;

        _checkmarkObj.SetActive(false);

        _button.onClick.AddListener(() => onSelectCallback.Invoke(this));
    }

    public void CheckmarkSwitch(bool isActive)
    {
        _checkmarkObj.SetActive(isActive);
    }

    public async UniTask ChangeImage()
    {
        var matchingUnit = MainSystem.Instance.PlayerData.unit_formation.FirstOrDefault(unit => unit.slot_number == _slotNumber);

        if (matchingUnit == null)
        {
            _image.sprite = await MainSystem.Instance.AddressableManager.LoadAssetAsync<Sprite>("Home/Sprites/monsterbuttonsq0");
            return;
        }

        var asset = matchingUnit.MasterAlly.icon_asset_address_square;
        _image.sprite = await MainSystem.Instance.AddressableManager.LoadAssetAsync<Sprite>(asset);
    }
}
