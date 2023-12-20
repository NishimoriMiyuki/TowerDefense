using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class WorkLevelUpButton : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _levelText, _goldText;

    [SerializeField]
    private Button _button;

    private bool _isLevelMax;

    private void Start()
    {
        _button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        if (_isLevelMax)
        {
            return;
        }

        GameManager.Instance.WorkLevelUp();
    }

    public void SetData(Work workData)
    {
        _levelText.text = "レベル" + workData.lv;
        _goldText.text = workData.lv_up_gold + "円";
    }

    public void LevelMax()
    {
        _isLevelMax = true;
        _levelText.text = "レベルマックス";
        _goldText.text = "";
    }
}