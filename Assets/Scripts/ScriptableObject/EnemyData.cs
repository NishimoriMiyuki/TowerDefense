using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MyScriptsbleObject/EnemyData")]
public class EnemyData : ScriptableObject
{
    public List<Enemy> EnemyList = new();
}

[Serializable]
public class Enemy
{
    public int id;
    public string asset_address;
    public int hp;
    public int attack;
    public float attack_interval_time;
    public float movement_speed;
    public int gold;
}