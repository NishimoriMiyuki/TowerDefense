using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MyScriptsbleObject/AllyData")]
public class AllyData : ScriptableObject
{
    public List<Ally> AllyList = new();
}

public enum attack_type
{
    none,
    area,
    single,
}

[Serializable]
public class Ally
{
    public int id;
    public int hp;
    public int attack;
    public attack_type attack_Type;
    public float movement_speed;
    public float attack_interval_time;
    public int max_lv;
    public float growth_hp;
    public float growth_attack;
    public int summon_gold;
    public string model_asset_address;
    public string icon_asset_address;
    public string icon_gray_asset_address;
    public string icon_asset_address_square;
    public int powerup_group_id;
    public int unlock_xp;
}