using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;

public class UnitScrollView : MonoBehaviour
{
    [SerializeField]
    private UnitObject _unitObject;

    [SerializeField]
    private RectTransform _content;

    private int _selectedUnitId = -1;
    public int SelectedUnitId => _selectedUnitId;

    private List<UnitObject> _cellList = new();
    private Action _onUnitSelected;

    public async UniTask Init(Action onUnitSelected)
    {
        _onUnitSelected = onUnitSelected;
        await CreateUnitObject();
    }

    private async UniTask CreateUnitObject()
    {
        var playerUnits = MainSystem.Instance.PlayerData.unit;

        foreach (var unit in playerUnits)
        {
            var instance = Instantiate(_unitObject, _content);
            await instance.Init(unit, OnSelect);
            _cellList.Add(instance);
        }
    }

    private void OnSelect(UnitObject unitObject)
    {
        MainSystem.Instance.SoundManager.PlaySe(ConstAddress.ButtonPush_3).Forget();

        var isCheck = _selectedUnitId == unitObject.PlayerUnitData.MasterAlly.id;

        AllRemoveCheckmark();

        // 選択していなかったら
        if (!isCheck)
        {
            unitObject.CheckmarkSwitch(true);
            _selectedUnitId = unitObject.PlayerUnitData.MasterAlly.id;
            _onUnitSelected.Invoke();
            return;
        }
        _selectedUnitId = -1;
    }

    private void AllRemoveCheckmark()
    {
        foreach (var cell in _cellList)
        {
            cell.CheckmarkSwitch(false);
        }
    }

    public void OnOrganized()
    {
        _selectedUnitId = -1;
        AllRemoveCheckmark();
    }
}
