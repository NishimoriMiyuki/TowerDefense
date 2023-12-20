using TMPro;
using UnityEngine;

public class SummonGold : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _currentGoldText, _maxGoldText;

    private const float DEFAULT_SPEED = 7;
    private const int DEFAULT_GOLD = 100;

    private float _speed;
    private float _currentGold;
    private int _maxGold;

    public float CurrentGold => _currentGold;

    public void Init()
    {
        _speed = DEFAULT_SPEED;
        _currentGold = 0;
        _maxGold = DEFAULT_GOLD;
    }

    public void DecreaseFunds(int gold)
    {
        _currentGold -= gold;
    }

    public void SpeedUp(float multiplier, int maxGold)
    {
        _speed = DEFAULT_SPEED * multiplier;
        _maxGold = maxGold;
        _maxGoldText.text = _maxGold.ToString();
    }

    private void Update()
    {
        if (_currentGold >= _maxGold)
        {
            return;
        }

        _currentGold += Time.deltaTime * _speed;
        _currentGoldText.text = _currentGold.ToString("F0");
    }
}