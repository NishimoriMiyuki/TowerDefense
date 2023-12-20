using UnityEngine;
using System;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    [SerializeField]
    private RectTransform _canvasRectTransform;

    [SerializeField]
    private GameObject _shield;

    private GameObject _dialogPanel;

    private Action _actionOK = null;
    private Action _actionCancel = null;

    private void Awake()
    {
        _shield.SetActive(false);
    }

    public async UniTask Alert(string mess, Action funcOk = null)
    {
        _shield.SetActive(true);

        _dialogPanel = await Instantiate("Prefabs/Dialog/DialogPanel");

        var dialogText = await AddObj("Prefabs/Dialog/DialogText");
        dialogText.GetComponent<TextMeshProUGUI>().text = mess;

        _actionOK = funcOk;
        var okPanel = await AddObj("Prefabs/Dialog/OKPanel");
        okPanel.transform.Find("OkButton").GetComponent<Button>().onClick.AddListener(ClickOkButton);
    }

    public async UniTask Confirm(string mess, Action funcOk = null, Action funcCancel = null)
    {
        _shield.SetActive(true);

        _dialogPanel = await Instantiate("Prefabs/Dialog/DialogPanel");

        var dialogText = await AddObj("Prefabs/Dialog/DialogText");
        dialogText.GetComponent<TextMeshProUGUI>().text = mess;

        _actionOK = funcOk;
        _actionCancel = funcCancel;
        GameObject buttonPanel = await AddObj("Prefabs/Dialog/OkCancelPanel");
        buttonPanel.transform.Find("OkButton").GetComponent<Button>().onClick.AddListener(ClickOkButton);
        buttonPanel.transform.Find("CancelButton").GetComponent<Button>().onClick.AddListener(ClickCancelButton);
    }

    private async UniTask<GameObject> Instantiate(string address)
    {
        var instance = await MainSystem.Instance.AddressableManager.InstantiateAsync(address);
        instance.transform.SetParent(_canvasRectTransform, false);
        instance.transform.localScale = Vector3.one;
        instance.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
        return instance;
    }

    private void ClickOkButton()
    {
        Close();
        if (_actionOK != null)
        { 
            _actionOK();
        }
    }

    private void ClickCancelButton()
    {
        MainSystem.Instance.SoundManager.PlaySe(ConstAddress.CancelButton_0).Forget();

        Close();
        if (_actionCancel != null)
        {
            _actionCancel();
        }
    }

    private async UniTask<GameObject> AddObj(string add)
    {
        GameObject obj = await MainSystem.Instance.AddressableManager.InstantiateAsync(add);
        obj.transform.SetParent(_dialogPanel.transform, false);
        return obj;
    }

    private void Close()
    {
        _shield.SetActive(false);
        Destroy(_dialogPanel);
    }
}
