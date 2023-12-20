using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public enum GameState
{
    Init, // 構築
    Start, // ゲームスタート
    Play, // ゲームプレイ
    GameClear, // ゲームクリア
    GameOver, // ゲームオーバー
}

public class GameManager : SingletonBehaviourSceneOnly<GameManager>
{
    [SerializeField]
    private BattleStageScreen _battleStageScreen;

    [SerializeField]
    private SummonGold _summonGold;

    [SerializeField]
    private List<Transform> _enemySpawnPoint, _allySpawnPoint;

    [SerializeField]
    private GameUIManager _gameUIManager;

    private GameState _state;
    private Queue<Work> _workDataQueue = new();
    private BattleStage _battleStageData;
    private CancellationTokenSource _cts;
    private CancellationToken _token;

    public float CurrentGold => _summonGold.CurrentGold;

    public async UniTask Init(BattleStage battleStageData, string stageName)
    {
        _state = GameState.Init;
        Debug.Log(_state);

        _cts = new CancellationTokenSource();
        _token = _cts.Token;

        _battleStageData = battleStageData;

        await _battleStageScreen.Init(battleStageData);

        var workDataList = MainSystem.Instance.Master.WorkData
            .Where(_ => _.group_id == battleStageData.work_group_id)
            .OrderBy(_ => _.lv);
        _workDataQueue = new Queue<Work>(workDataList);
        _gameUIManager.Init(_workDataQueue, stageName);
        _summonGold.Init();
    }

    public void GameStart()
    {
        _state = GameState.Start;
        Debug.Log("GameState = Start");

        CreateEnemy(_token).Forget();
        GamePlay();
    }

    private void GamePlay()
    {
        _state = GameState.Play;
        Debug.Log("GameState = Play");
    }

    public void GameClear()
    {
        _state = GameState.GameClear;
        Debug.Log("GameState = Clear ");

        MainSystem.Instance.PlayerData.AddXp(_battleStageData.reward_xp);

        _cts.Cancel();

        var enemys = FindObjectsByType<EnemyController>(FindObjectsSortMode.None);
        if (enemys != null)
        {
            foreach (var enemy in enemys)
            {
                enemy.Die();
            }
        }

        _gameUIManager.PlayGameEndUIAnim();
        _gameUIManager.OpenClearPanel(_battleStageData.reward_xp).Forget();
    }

    public void GameOver()
    {
        _state = GameState.GameOver;
        Debug.Log("GameState = GameOver");

        _cts.Cancel();

        var allys = FindObjectsByType<AllyController>(FindObjectsSortMode.None);
        if (allys != null)
        {
            foreach (var ally in allys)
            {
                ally.Die();
            }
        }

        _gameUIManager.PlayGameEndUIAnim();
        _gameUIManager.OpenGameOverPanel().Forget();
    }

    public async UniTask AllySummon(int allyId)
    {
        var playerUnitData = MainSystem.Instance.PlayerData.unit.FirstOrDefault(_ => _.ally_id == allyId);

        if (_summonGold.CurrentGold < playerUnitData.MasterAlly.summon_gold)
        {
            Debug.Log("お金足りない");
            return;
        }

        var randomTransform = _allySpawnPoint.OrderBy(_ => Guid.NewGuid()).FirstOrDefault();
        _summonGold.DecreaseFunds(playerUnitData.MasterAlly.summon_gold);
        var instance = await MainSystem.Instance.AddressableManager.InstantiateAsync(playerUnitData.MasterAlly.model_asset_address, cancellationToken: _token);
        instance.transform.position = randomTransform.position;
        instance.GetComponentInChildren<AllyController>().Init(playerUnitData);

        Debug.Log("味方召喚 AllyId = " + playerUnitData.MasterAlly.id);
    }

    public void WorkLevelUp()
    {
        var workData = _workDataQueue.Peek();

        if (_summonGold.CurrentGold < workData.lv_up_gold)
        {
            return;
        }

        MainSystem.Instance.SoundManager.PlaySe(ConstAddress.WoekLevelUp).Forget();

        _summonGold.DecreaseFunds(workData.lv_up_gold);
        _summonGold.SpeedUp(workData.speed, workData.max_gold);
        _workDataQueue.Dequeue();

        if (_workDataQueue.TryPeek(out var nextWorkData))
        {
            _gameUIManager.SetNextWorkLevel(nextWorkData);
        }
        else
        {
            _gameUIManager.WorkLevelMax();
        }
    }

    private async UniTaskVoid CreateEnemy(CancellationToken token)
    {
        var enemySpawnGroupData = MainSystem.Instance.Master.EnemySpawnData
            .Where(_ => _.group_id == _battleStageData.enemy_spawn_group_id)
            .OrderBy(_ => _.order);

        Queue<EnemySpawn> enemySpawnDataQueue = new(enemySpawnGroupData);

        while (true)
        {
            var enemySpawnData = enemySpawnDataQueue.Dequeue();
            await UniTask.Delay(TimeSpan.FromSeconds(enemySpawnData.wait_time), cancellationToken: token);
            var enemyData = MainSystem.Instance.Master.EnemyData.FirstOrDefault(_ => _.id == enemySpawnData.enemy_id);
            var randomTransform = _enemySpawnPoint.OrderBy(_ => Guid.NewGuid()).FirstOrDefault();
            var instance = await MainSystem.Instance.AddressableManager.InstantiateAsync(enemyData.asset_address, cancellationToken: _token);
            instance.transform.position = randomTransform.position;
            instance.GetComponentInChildren<EnemyController>().Init(enemyData);
            enemySpawnDataQueue.Enqueue(enemySpawnData);
        }
    }
}
