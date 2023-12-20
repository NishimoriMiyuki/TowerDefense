using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

[Serializable]
public class PlayerData
{
    public int xp; // 所持xp
    public List<PlayerUnitData> unit = new(); // 所持キャラクター
    public List<int> stage = new(); // クリアステージ
    public List<PlayerUnitFormation> unit_formation = new(); // ユニット編成

    public void ReducedXp(int xp)
    {
        this.xp -= xp;
    }

    public void AddXp(int xp)
    {
        this.xp += xp;
    }
}

[Serializable]
public class PlayerUnitData
{
    public int ally_id;
    public int lv;

    public Ally MasterAlly => MainSystem.Instance.Master.AllyData.FirstOrDefault(_ => _.id == ally_id);

    public Powerup MasterPowerup => MainSystem.Instance.Master.PowerupData
            .Where(_ => _.group_id == MasterAlly.powerup_group_id)
            .FirstOrDefault(_ => _.lv == NextLv);

    public int NextLv => lv + 1;
    public float Hp => Mathf.Ceil(MasterAlly.hp * Mathf.Pow(MasterAlly.growth_hp, lv));
    public float Attack => Mathf.Ceil(MasterAlly.attack * Mathf.Pow(MasterAlly.growth_attack, lv));
    public float NextLvHp => Mathf.Ceil(MasterAlly.hp * Mathf.Pow(MasterAlly.growth_hp, NextLv));
    public float NextLvAttack => Mathf.Ceil(MasterAlly.attack * Mathf.Pow(MasterAlly.growth_attack, NextLv));

    public void LvUp()
    {
        lv++;
    }
}

[Serializable]
public class PlayerUnitFormation
{
    public int ally_id;
    public int slot_number;

    public Ally MasterAlly => MainSystem.Instance.Master.AllyData.FirstOrDefault(_ => _.id == ally_id);
}

public class SaveDataManager
{
    private const string PLAYER_DATA_KEY = "PlayerData";

    public void Save()
    {
        var json = JsonUtility.ToJson(MainSystem.Instance.PlayerData);
        PlayerPrefs.SetString(PLAYER_DATA_KEY, json);
        PlayerPrefs.Save();
    }

    public void Load()
    {
        if (!PlayerPrefs.HasKey(PLAYER_DATA_KEY))
        {
            CreateInitData();
            Save();
        }

        var playerData = PlayerPrefs.GetString(PLAYER_DATA_KEY);
        MainSystem.Instance.PlayerData = JsonUtility.FromJson<PlayerData>(playerData);
    }

    /// <summary>
    /// プレイヤーの初期データ
    /// </summary>
    private void CreateInitData()
    {
        MainSystem.Instance.PlayerData.unit.Add(new PlayerUnitData { ally_id = 101, lv = 1 });

        MainSystem.Instance.PlayerData.unit_formation.Add(new PlayerUnitFormation { ally_id = 101, slot_number = 0 });
    }
}
