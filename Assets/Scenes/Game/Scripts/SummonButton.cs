using Cysharp.Threading.Tasks;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SummonButton : MonoBehaviour
{
    [SerializeField]
    private Button _button;

    [SerializeField]
    private TextMeshProUGUI _summonGoldText;

    [SerializeField]
    private Slider _slider;

    [SerializeField]
    private Image _iconImage;

    [SerializeField]
    private GameObject _goldTexts;

    private int _slotNumber;

    private Ally _allyData;

    public async UniTaskVoid Init(int slotNumber)
    {
        _slider.gameObject.SetActive(false);
        _goldTexts.SetActive(false);
        _slider.value = 0;
        _button.interactable = false;
        _slotNumber = slotNumber;


        var matchingUnit = MainSystem.Instance.PlayerData.unit_formation.FirstOrDefault(_ => _.slot_number == _slotNumber);
        if (matchingUnit == null)
        {
            return;
        }

        _summonGoldText.text = matchingUnit.MasterAlly.summon_gold.ToString();
        _slider.maxValue = matchingUnit.MasterAlly.summon_gold;
        _iconImage.sprite = await MainSystem.Instance.AddressableManager.LoadAssetAsync<Sprite>(matchingUnit.MasterAlly.icon_asset_address);
        SpriteState spriteState = _button.spriteState;
        spriteState.disabledSprite = await MainSystem.Instance.AddressableManager.LoadAssetAsync<Sprite>(matchingUnit.MasterAlly.icon_gray_asset_address);
        _button.spriteState = spriteState;
        _slider.gameObject.SetActive(true);
        _goldTexts.SetActive(true);
        _allyData = matchingUnit.MasterAlly;

        _button.onClick.AddListener(() =>
        {
            GameManager.Instance.AllySummon(matchingUnit.ally_id).Forget();
        });

    }

    private void Update()
    {
        if (_allyData == null)
        {
            return;
        }

        _slider.value = GameManager.Instance.CurrentGold;

        if (_slider.value == _slider.maxValue)
        {
            _button.interactable = true;
        }
        else
        {
            _button.interactable = false;
        }
    }
}
