using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class AllySummonPanel : MonoBehaviour
{
    [SerializeField]
    private List<SummonButton> _summonButtonList;

    public void Init()
    {
        SetAllyData();
    }

    private void SetAllyData()
    {
        for (int i = 0; i < _summonButtonList.Count; i++)
        {
            _summonButtonList[i].Init(i).Forget();
        }
    }
}
