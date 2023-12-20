using Cysharp.Threading.Tasks;
using UnityEngine;
using System.Linq;

public class BattleStageScreen : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer _bgSpriteRenderer;

    [SerializeField]
    private EnemyCastle _enemyCastle;

    [SerializeField]
    private AllyCastle _allyCastle;

    AddressableManager _abManager => MainSystem.Instance.AddressableManager;

    public async UniTask Init(BattleStage battleStageData)
    {
        var castleData = MainSystem.Instance.Master.CastleData.Where(_ => _.group_id == battleStageData.castle_group_id);
        var enemyCastleData = castleData.FirstOrDefault(_ => _.type == Castle.Type.Enemy);
        var allyCastleData = castleData.FirstOrDefault(_ => _.type == Castle.Type.Ally);

        _enemyCastle.Init(enemyCastleData);
        _allyCastle.Init(allyCastleData);

        _bgSpriteRenderer.sprite = await _abManager.LoadAssetAsync<Sprite>(battleStageData.background_address);
    }
}
