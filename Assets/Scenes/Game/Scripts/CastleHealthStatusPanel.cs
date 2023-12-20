using TMPro;
using UnityEngine;

public class CastleHealthStatusPanel : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _enemyCastleCurrentHp, _enemyCastleMaxHp;

    [SerializeField]
    private TextMeshProUGUI _allyCastleCurrentHp, _allyCastleMaxHp;

    public void SetMaxHp(int maxHp, Castle.Type type)
    {
        switch (type)
        {
            case Castle.Type.Enemy:
                _enemyCastleMaxHp.text = maxHp.ToString();
                _enemyCastleCurrentHp.text = maxHp.ToString();
                break;
            case Castle.Type.Ally:
                _allyCastleMaxHp.text = maxHp.ToString();
                _allyCastleCurrentHp.text = maxHp.ToString();
                break;
        }
    }

    public void UpdateHealthStatus(int hp, Castle.Type type)
    {
        switch (type)
        {
            case Castle.Type.Enemy:
                _enemyCastleCurrentHp.text = hp.ToString();
                break;
            case Castle.Type.Ally:
                _allyCastleCurrentHp.text = hp.ToString();
                break;
        }
    }
}
