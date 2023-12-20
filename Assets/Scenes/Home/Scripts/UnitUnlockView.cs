using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitUnlockView : MonoBehaviour
{
    [SerializeField]
    private RectTransform _content;

    [SerializeField]
    private UnitUnlockCell _cell;

    private List<UnitUnlockCell> _cellList = new();

    private Action _xpRefresh;

    public async UniTask Init(Action XpRefresh)
    {
        _xpRefresh = XpRefresh;

        await CreateCell(_xpRefresh);
    }

    private async UniTask CreateCell(Action XpRefresh)
    {
        var playerUnits = MainSystem.Instance.PlayerData.unit;
        var allAllys = MainSystem.Instance.Master.AllyData;

        var notPlayerUnit = allAllys.Where(ally => !playerUnits.Any(unit => unit.ally_id == ally.id));

        foreach (var allyData in notPlayerUnit)
        {
            var instance = Instantiate(_cell, _content);
            await instance.Init(allyData, XpRefresh);
            _cellList.Add(instance);
        }
    }

    public void Refresh()
    {
        foreach (var cell in _cellList)
        {
            Destroy(cell.gameObject);
        }

        _cellList.Clear();

        CreateCell(_xpRefresh).Forget();
    }
}
