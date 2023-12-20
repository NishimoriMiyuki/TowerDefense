using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour
{
    [SerializeField]
    private WorkLevelUpButton _workLevelUpButton;

    [SerializeField]
    private AllySummonPanel _allySummonPanel;

    [SerializeField]
    private TextMeshProUGUI _stageNameText;

    [SerializeField]
    private Animation _animation;

    [SerializeField]
    private Button _poseButton, _cancellationPoseButton;

    [SerializeField]
    private RectTransform _panelParent;

    [SerializeField]
    private GameObject _poseCanvas;

    public void Init(Queue<Work> workDataQueue, string stageName)
    {
        _workLevelUpButton.SetData(workDataQueue.Peek());
        _allySummonPanel.Init();

        _stageNameText.text = stageName;

        _poseButton.onClick.AddListener(OnClickPoseButton);
        _cancellationPoseButton.onClick.AddListener(OnClickCancellationPoseButton);

        _poseCanvas.SetActive(false);
    }

    public void WorkLevelMax()
    {
        _workLevelUpButton.LevelMax();
    }

    public void SetNextWorkLevel(Work nextWorkData)
    {
        _workLevelUpButton.SetData(nextWorkData);
    }

    public void PlayGameEndUIAnim()
    {
        _animation.Play();
    }

    private void OnClickPoseButton()
    {
        MainSystem.Instance.SoundManager.PlaySe(ConstAddress.ButtonPush_0).Forget();
        _poseButton.gameObject.SetActive(false);
        _poseCanvas.SetActive(true);
        Time.timeScale = 0;
    }

    private void OnClickCancellationPoseButton()
    {
        MainSystem.Instance.SoundManager.PlaySe(ConstAddress.BackButton_0).Forget();
        _poseButton.gameObject.SetActive(true);
        _poseCanvas.SetActive(false);
        Time.timeScale = 1;
    }

    public async UniTaskVoid OpenClearPanel(int rewardXp)
    {
        var clearPanel = await MainSystem.Instance.AddressableManager.InstantiateAsync("Game/Prefabs/ClearPanel", _panelParent);
        clearPanel.GetComponent<ClearPanel>().Init(rewardXp);
    }

    public async UniTaskVoid OpenGameOverPanel()
    {
        var gameOverPanel = await MainSystem.Instance.AddressableManager.InstantiateAsync("Game/Prefabs/GameOverPanel", _panelParent);
        gameOverPanel.GetComponent<GameOverPanel>().Init();
    }
}
