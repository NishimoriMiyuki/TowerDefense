using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StageSelectScrollView : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField]
    private ScrollRect _scrollRect;

    [SerializeField]
    private HorizontalLayoutGroup _layoutGroup;

    [SerializeField]
    private StageObject _stageObjPrefab;

    private RectTransform _content;
    private int _currentIndex;
    private List<float> _scrollPositionXs = new();
    private List<StageObject> _elements = new();
    private List<WorldStage> _worldStages;
    private bool _isDragging;

    private readonly Vector2 _elementMaxSize = new(250, 120);
    private readonly Vector2 _elementDefaultSize = new(200, 100);

    public WorldStage CurrenSelectedWorldStage => _worldStages[_currentIndex];
    public bool IsDragging => _isDragging;

    private WorldUnitController _worldUnitController;
    private GameObject _worldMapInstance;
    private Vector3 _defaultMainCameraPosition;

    public async UniTask Init(List<WorldStage> data)
    {
        _worldStages = data;
        _content = _scrollRect.content;
        _scrollRect.inertia = false;
        _defaultMainCameraPosition = Camera.main.transform.position;

        await CreateElement();
        _content.anchoredPosition = new(_scrollPositionXs[_currentIndex], _content.anchoredPosition.y);

        _worldMapInstance = await MainSystem.Instance.AddressableManager.InstantiateAsync("Home/Prefabs/WorldMap");

        _worldUnitController = FindAnyObjectByType<WorldUnitController>();
        _worldUnitController.gameObject.transform.position = new Vector3(_worldStages[_currentIndex].map_posX, _worldStages[_currentIndex].map_posY);
        Camera.main.transform.position = new Vector3(_worldStages[_currentIndex].map_posX + 1, _worldStages[_currentIndex].map_posY + 1, -10);
    }

    private async UniTask CreateElement()
    {
        for (int i = 0; i < _worldStages.Count; i++)
        {
            var instance = Instantiate(_stageObjPrefab, _content);
            instance.SetData(_worldStages[i]);
            instance.ChangeVisual(_elementDefaultSize, Color.gray);
            _scrollPositionXs.Add(-i * (_elementDefaultSize.x + _layoutGroup.spacing));
            _elements.Add(instance);
        }

        var firstElement = _elements.FirstOrDefault();
        await UniTask.NextFrame();
        firstElement.ChangeVisual(_elementMaxSize, Color.white);
        _currentIndex = 0;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _isDragging = true;

        foreach (var element in _elements)
        {
            element.ChangeVisual(_elementDefaultSize, Color.gray);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _isDragging = false;

        _content.anchoredPosition = new(_scrollPositionXs[_currentIndex], _content.anchoredPosition.y);
    }

    Queue<Vector3> roots = new();

    public void OnDrag(PointerEventData eventData)
    {
        float currentPos = _content.anchoredPosition.x;
        float closest = float.MaxValue;
        float targetPosX = 0;

        foreach (var posX in _scrollPositionXs)
        {
            float distance = Mathf.Abs(posX - currentPos);
            if (distance < closest)
            {
                closest = distance;
                targetPosX = posX;
            }
        }

        int previousIndex = _currentIndex;

        _elements[previousIndex].ChangeVisual(_elementDefaultSize, Color.gray);
        _elements.Where(_ => !_.IsSelected).ToList().ForEach(_ => _.ChangeVisual(_elementDefaultSize, Color.gray));

        int index = _scrollPositionXs.IndexOf(targetPosX);
        _elements[index].ChangeVisual(_elementMaxSize, Color.white);
        _currentIndex = index;

        if (previousIndex == _currentIndex)
        {
            return;
        }

        MainSystem.Instance.SoundManager.PlaySe(ConstAddress.Snap).Forget();
        roots.Enqueue(new Vector3(_worldStages[_currentIndex].map_posX, _worldStages[_currentIndex].map_posY));
        _worldUnitController.SetRoot(roots).Forget();
    }

    private void OnDestroy()
    {
        if (Camera.main != null)
        {
            Camera.main.transform.position = _defaultMainCameraPosition;
        }

        if (_worldMapInstance != null)
        {
            MainSystem.Instance.AddressableManager.ReleaseInstance(_worldMapInstance);
        }
    }
}
