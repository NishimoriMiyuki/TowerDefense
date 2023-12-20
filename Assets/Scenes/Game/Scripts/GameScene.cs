using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class GameScene : SceneBase
{
    protected override async UniTask OnInitialize(object args)
    {
        await base.OnInitialize(args);
        Debug.Log("OnInitialize GameScene");

        var worldStage = args as WorldStage;

        if (worldStage == null)
        {
            //Debug.LogWarning("WorldStageDataが入ってない");
            //return;

            // GameSceneから初められるように仮で入れとく
            worldStage = MainSystem.Instance.Master.WorldStageData
                .FirstOrDefault();
        }

        var stageName = worldStage.stage_name;
        var battleStageData = MainSystem.Instance.Master.BattleStageData
            .FirstOrDefault(_ => _.id == worldStage.battle_stage_id);

        if (battleStageData == null)
        {
            Debug.LogWarning("BattleStageDataが入ってない");
            return;
        }

        MainSystem.Instance.SoundManager.PlayBgm(battleStageData.bgm_address).Forget();
        await GameManager.Instance.Init(battleStageData, stageName);
    }

    protected override async UniTask OnFadeEndAction(object args)
    {
        await base.OnFadeEndAction(args);

        GameManager.Instance.GameStart();
    }
}
