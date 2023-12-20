using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MyScriptsbleObject/EnemySpawnData")]
public class EnemySpawnData : ScriptableObject
{
    public List<EnemySpawn> EnemySpawnList = new();
}

[Serializable]
public class EnemySpawn
{
    public int id;
    public int group_id;
    public int order;
    public int enemy_id;
    public float wait_time;
}