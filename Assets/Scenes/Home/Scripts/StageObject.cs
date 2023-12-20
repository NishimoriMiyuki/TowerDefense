using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageObject : MonoBehaviour
{
    [SerializeField]
    private LayoutElement _layoutElement;

    [SerializeField]
    private Image _image;

    [SerializeField]
    private TextMeshProUGUI _stamina, _name, _title;

    public bool IsSelected => _image.color == Color.white;

    public void SetData(WorldStage worldStage)
    {
        _stamina.text = (-worldStage.stamina).ToString();
        _name.text = worldStage.stage_name;
    }

    public void ChangeVisual(Vector2 size, Color color)
    {
        _layoutElement.minHeight = size.y;
        _layoutElement.minWidth = size.x;

        if (color == Color.gray)
        {
            var imageColor = _image.color;
            imageColor = new Color(color.r, color.g, color.b, 0.6f);
            _image.color = imageColor;

            var staminaColor = _stamina.color;
            staminaColor.a = 0.6f;
            _stamina.color = staminaColor;

            var nameColor = _name.color;
            nameColor.a = 0.6f;
            _name.color = nameColor;

            var titleColor = _title.color;
            titleColor.a = 0.6f;
            _title.color = titleColor;
        }
        else
        {
            _image.color = color;

            var staminaColor = _stamina.color;
            staminaColor.a = 1f;
            _stamina.color = staminaColor;

            var nameColor = _name.color;
            nameColor.a = 1f;
            _name.color = nameColor;

            var titleColor = _title.color;
            titleColor.a = 1f;
            _title.color = titleColor;
        }
    }
}