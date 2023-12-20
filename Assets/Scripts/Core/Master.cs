using System.Collections.Generic;
using UnityEngine;

public class Master : MonoBehaviour
{
    [SerializeField]
    private AllyData _allyData;
    public List<Ally> AllyData => _allyData.AllyList;

    [SerializeField]
    private BattleStageData _battleStageData;
    public List<BattleStage> BattleStageData => _battleStageData.BattleStageList;

    [SerializeField]
    private EnemyData _enemyData;
    public List<Enemy> EnemyData => _enemyData.EnemyList;

    [SerializeField]
    private EnemySpawnData _enemyspawnData;
    public List<EnemySpawn> EnemySpawnData => _enemyspawnData.EnemySpawnList;

    [SerializeField]
    private WorldStageData _worldStageData;
    public List<WorldStage> WorldStageData => _worldStageData.WorldStageList;

    [SerializeField]
    private WorkData _workData;
    public List<Work> WorkData => _workData.WorkList;

    [SerializeField]
    private CastleData _castleData;
    public List<Castle> CastleData => _castleData.CastleList;

    [SerializeField]
    private PowerupData _powerupData;
    public List<Powerup> PowerupData => _powerupData.PowerupList;
}
