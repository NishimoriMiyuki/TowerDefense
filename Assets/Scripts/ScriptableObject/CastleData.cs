using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MyScriptsbleObject/CastleData")]
public class CastleData : ScriptableObject
{
    public List<Castle> CastleList = new();
}

[Serializable]
public class Castle
{
    public enum Type
    {
        Enemy,
        Ally,
    }

    public int id;
    public int group_id;
    public string asset_address;
    public int hp;
    public Type type;
}
