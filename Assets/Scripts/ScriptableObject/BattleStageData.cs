using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(menuName = "MyScriptsbleObject/BattleStageData")]
public class BattleStageData : ScriptableObject
{
    public List<BattleStage> BattleStageList = new();
}

[Serializable]
public class BattleStage
{
    public int id;
    public int reward_xp;
    public int enemy_spawn_group_id;
    public int work_group_id;
    public int castle_group_id;
    public string bgm_address;
    public string background_address;
}
