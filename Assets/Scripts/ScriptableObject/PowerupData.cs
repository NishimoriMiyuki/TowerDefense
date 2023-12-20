using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MyScriptsbleObject/PowerupData")]
public class PowerupData : ScriptableObject
{
    public List<Powerup> PowerupList = new();
}

[Serializable]
public class Powerup
{
    public int id;
    public int group_id;
    public int requird_xp;
    public int lv;
}
