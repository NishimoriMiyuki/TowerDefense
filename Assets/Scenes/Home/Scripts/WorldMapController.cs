using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class WorldMapController : MonoBehaviour
{
    [SerializeField]
    private GameObject _circlePrefab;

    private void Start()
    {
        CreateIndicatePositionPrefab();
    }

    public void CreateIndicatePositionPrefab()
    {
        var createPosition = MainSystem.Instance.Master.WorldStageData.Select(_ => new Vector3(_.map_posX, _.map_posY));

        foreach (var pos in createPosition)
        {
            var instance = Instantiate(_circlePrefab, transform);
            instance.transform.position = pos;
        }
    }
}
