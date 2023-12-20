using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class UnitLevelUpView : MonoBehaviour
{
    [SerializeField]
    private RectTransform _content;

    [SerializeField]
    private UnitLvUpCell _cell;

    private List<UnitLvUpCell> _cellList = new();
    private Action _xpRefresh;

    public async UniTask Init(Action xpRefresh)
    {
        _xpRefresh = xpRefresh;

        await CreateCell(_xpRefresh);
    }

    private async UniTask CreateCell(Action xpRefresh)
    {
        var playerUnits = MainSystem.Instance.PlayerData.unit;

        foreach (var unit in playerUnits)
        {
            var instance = Instantiate(_cell, _content);
            await instance.Init(unit, xpRefresh);
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
