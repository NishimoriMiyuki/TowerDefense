using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(menuName = "MyScriptsbleObject/WorldStageData")]
public class WorldStageData : ScriptableObject
{
    public List<WorldStage> WorldStageList = new();
}

[Serializable]
public class WorldStage
{
    public int id;
    public string stage_name;
    public int stamina;
    public float map_posX;
    public float map_posY;
    public int battle_stage_id;
    public int order;
}