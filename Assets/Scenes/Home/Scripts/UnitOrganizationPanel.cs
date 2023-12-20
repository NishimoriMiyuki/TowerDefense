using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class UnitOrganizationPanel : PanelBase
{
    [SerializeField]
    private UnitScrollView _unitScrollView;

    [SerializeField]
    private UnitDeckView _unitDeckView;

    private PlayerData _playerData;

    protected override async UniTask OnInitialize(object args)
    {
        await base.OnInitialize(args);

        _playerData = MainSystem.Instance.PlayerData;

        await _unitScrollView.Init(OnSelected);
        await _unitDeckView.Init(OnSelected);
    }

    private void OnSelected()
    {
        if (_unitDeckView.SelectedSlotNumber == -1 || _unitScrollView.SelectedUnitId == -1)
        {
            return;
        }

        var newData = new PlayerUnitFormation { ally_id = _unitScrollView.SelectedUnitId, slot_number = _unitDeckView.SelectedSlotNumber };

        var slotMatch = _playerData.unit_formation.FirstOrDefault(data => data.slot_number == newData.slot_number);
        if (slotMatch != null)
        {
            var matchingAlly = _playerData.unit_formation.FirstOrDefault(data => data.ally_id == newData.ally_id);
            if (matchingAlly != null)
            {
                slotMatch.slot_number = matchingAlly.slot_number;
            }
        }

        var allyIdMatch = _playerData.unit_formation.FirstOrDefault(data => data.ally_id == newData.ally_id);
        if (allyIdMatch != null)
        {
            _playerData.unit_formation.Remove(allyIdMatch);
        }

        _playerData.unit_formation.Add(newData);

        _unitDeckView.OnOrganized();
        _unitScrollView.OnOrganized();
    }
}
