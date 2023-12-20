using UnityEngine;

public class MainSystem : SingletonBehaviour<MainSystem>
{
    [SerializeField]
    private AppSceneManager _appSceneManager;
    public AppSceneManager AppSceneManager => _appSceneManager;

    [SerializeField]
    private FadeManager _fadeManager;
    public FadeManager FadeManager => _fadeManager;

    [SerializeField]
    private AddressableManager _addressableManager;
    public AddressableManager AddressableManager => _addressableManager;

    [SerializeField]
    private SoundManager _soundManager;
    public SoundManager SoundManager => _soundManager;

    [SerializeField]
    private Master _master;
    public Master Master => _master;

    [SerializeField]
    private DialogManager _dialogManager;
    public DialogManager DialogManager => _dialogManager;

    private PlayerData _playerData = new();
    public PlayerData PlayerData { get => _playerData; set => _playerData = value; }

    public SaveDataManager SaveDataManager = new();

    protected override void Awake()
    {
        base.Awake();

        SaveDataManager.Load();
    }

    private void OnApplicationQuit()
    {
        SaveDataManager.Save();
    }
}
