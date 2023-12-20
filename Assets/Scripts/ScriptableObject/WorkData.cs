using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MyScriptsbleObject/WorkData")]
public class WorkData : ScriptableObject
{
    public List<Work> WorkList = new();
}

[Serializable]
public class Work
{
    public int id;
    public int group_id;
    public int lv;
    public int lv_up_gold;
    public int max_gold;
    public float speed;
}