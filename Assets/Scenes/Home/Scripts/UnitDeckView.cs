using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UnitDeckView : MonoBehaviour
{
    [SerializeField]
    private List<UnitSlotObject> _unitSlotObjects;

    [SerializeField]
    private Button _removeButton;

    private int _selectedSlotNumber = -1;
    public int SelectedSlotNumber => _selectedSlotNumber;

    private Action _onSlotSelected;

    public async UniTask Init(Action onSlotSelected)
    {
        for (int i = 0; i < _unitSlotObjects.Count; i++)
        {
            int slotNumber = i;
            _unitSlotObjects[i].Init(slotNumber, OnSelect);
        }

        await UpdateSlot();

        _onSlotSelected = onSlotSelected;
        _removeButton.onClick.AddListener(OnClickRemoveButton);
    }

    private void OnClickRemoveButton()
    {
        if (_selectedSlotNumber == -1)
        {
            Debug.Log("-1だった");
            return;
        }

        MainSystem.Instance.SoundManager.PlaySe(ConstAddress.CancelButton_0).Forget();

        var matchUnit = MainSystem.Instance.PlayerData.unit_formation.FirstOrDefault(_ => _.slot_number == _selectedSlotNumber);
        MainSystem.Instance.PlayerData.unit_formation.Remove(matchUnit);
        _selectedSlotNumber = -1;
        AllRemoveCheckmark();
        UpdateSlot().Forget();
    }

    private void OnSelect(UnitSlotObject unitSlotObject)
    {
        MainSystem.Instance.SoundManager.PlaySe(ConstAddress.ButtonPush_3).Forget();

        var isCheck = _selectedSlotNumber == unitSlotObject.SlotNumber;

        AllRemoveCheckmark();

        // 選択していなかったら
        if (!isCheck)
        {
            unitSlotObject.CheckmarkSwitch(true);
            _selectedSlotNumber = unitSlotObject.SlotNumber;
            _onSlotSelected.Invoke();
            return;
        }
        _selectedSlotNumber = -1;
    }

    private void AllRemoveCheckmark()
    {
        foreach (var cell in _unitSlotObjects)
        {
            cell.CheckmarkSwitch(false);
        }
    }

    public void OnOrganized()
    {
        UpdateSlot().Forget();

        _selectedSlotNumber = -1;
        AllRemoveCheckmark();
    }

    private async UniTask UpdateSlot()
    {
        for (int i = 0; i < _unitSlotObjects.Count; i++)
        {
            await _unitSlotObjects[i].ChangeImage();
        }
    }
}
