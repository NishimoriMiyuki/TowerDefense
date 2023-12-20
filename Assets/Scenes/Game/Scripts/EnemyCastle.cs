using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Playables;

public class EnemyCastle : MonoBehaviour, IDamageable
{
    [SerializeField]
    private CastleHealthStatusPanel _castleHealthStatusView;

    [SerializeField]
    private PlayableDirector _playableDirector;

    private Castle _castleData;
    private CancellationToken _token;

    private int _hp;
    private bool _isInvincible;
    private Vector2 _initPosition;

    public void Init(Castle castleData)
    {
        _castleData = castleData;

        _hp = _castleData.hp;
        _initPosition = transform.position;

        _token = this.GetCancellationTokenOnDestroy();

        _castleHealthStatusView.SetMaxHp(_castleData.hp, _castleData.type);
    }

    public async UniTaskVoid TakeDamage(int damage)
    {
        Debug.Log("ダメージ食う" + gameObject.name + "残りHP = " + _hp);

        transform.DOKill(this);
        transform.position = _initPosition;

        float duration = 0.3f;
        float strength = 0.2f;
        int vibrato = 10;
        float randomness = 100;

        await transform.DOShakePosition(duration, strength, vibrato, randomness).WithCancellation(_token);

        _hp = Math.Max(_hp - damage, 0);

        _castleHealthStatusView.UpdateHealthStatus(_hp, _castleData.type);

        if (_hp <= 0)
        {
            Death();
        }
    }

    private void Death()
    {
        if (_isInvincible)
        {
            return;
        }

        _isInvincible = true;

        _playableDirector.Play();

        GameManager.Instance.GameClear();
    }
}
