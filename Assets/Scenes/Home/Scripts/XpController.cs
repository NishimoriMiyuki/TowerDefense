using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class XpController : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _xpText;

    public void Init()
    {
        Refresh();
    }

    public void Refresh()
    {
        _xpText.text = MainSystem.Instance.PlayerData.xp.ToString();
    }
}
