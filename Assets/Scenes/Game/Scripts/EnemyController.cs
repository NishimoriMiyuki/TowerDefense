using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class EnemyController : MonoBehaviour, IDamageable
{
    public enum State
    {
        Searching, // 索敵
        Attack, // 攻撃
        Cooldown, // 攻撃後のクールタイム
        Death,
    }

    [SerializeField]
    private AttackRange _attackRange;

    [SerializeField]
    private AttackParticle _attackParticle;

    [SerializeField]
    private Rigidbody2D _rigidbody;

    [SerializeField]
    private Animator _animator;

    [SerializeField]
    private CircleCollider2D _contactCollider;

    private Enemy _enemyData;

    private State _state;
    private int _hp;
    private int _knockbackCount;
    private bool _isKnockback;
    private CancellationToken _token;

    public void Init(Enemy enemyData)
    {
        _enemyData = enemyData;

        _hp = _enemyData.hp;

        _token = this.GetCancellationTokenOnDestroy();

        _attackRange.OnContact = OnContact;
        _attackParticle.SetAttackPower(enemyData.attack);

        Searching();
    }

    private void Update()
    {
        if (_isKnockback)
        {
            return;
        }

        if (_state == State.Searching)
        {
            Walk();
        }
    }

    public async UniTaskVoid TakeDamage(int damage)
    {
        _animator.SetInteger("StateNum", 3);

        if (_knockbackCount == 3)
        {
            _isKnockback = true;
            _knockbackCount = 0;

            float distance = -2.5f;
            float jumpPower = 1f;
            float duration = 1f;
            int numJumps = 3;
            Vector2 targetPosition = new(transform.parent.position.x + distance, transform.parent.position.y);

            _rigidbody.DOKill(false);
            await _rigidbody.DOJump(targetPosition, jumpPower, numJumps, duration).WithCancellation(_token);

            _isKnockback = false;
        }

        _knockbackCount++;

        Debug.Log("ダメージをくらう" + transform.parent.name + "残りHP = " + _hp);

        _hp -= damage;

        if (_hp <= 0)
        {
            Death();
        }
    }

    private void OnContact()
    {
        if (_state != State.Searching)
        {
            return;
        }

        Attack();
    }

    private void Searching()
    {
        if (_state == State.Death)
        {
            return;
        }

        _state = State.Searching;

        _attackRange.gameObject.SetActive(true);
        _attackParticle.DamageAlreadyGiven = false;
    }

    private void Walk()
    {
        _animator.SetInteger("StateNum", 1);
        transform.parent.Translate(_enemyData.movement_speed * Time.deltaTime, 0, 0);
    }

    private void Attack()
    {
        _state = State.Attack;

        _animator.SetInteger("StateNum", 2);

        Cooldown().Forget();
    }

    private async UniTaskVoid Cooldown()
    {
        _state = State.Cooldown;

        _attackRange.gameObject.SetActive(false);

        await UniTask.Delay(TimeSpan.FromSeconds(_enemyData.attack_interval_time), cancellationToken: _token);

        Searching();
    }

    private void Death()
    {
        _state = State.Death;

        _contactCollider.enabled = false;
        _attackRange.gameObject.SetActive(false);

        _animator.SetInteger("StateNum", 4);
    }

    #region AnimationEvent

    private void Idle()
    {
        _animator.SetInteger("StateNum", 0);
    }

    public void Die()
    {
        UniTask.Void(async () =>
        {
            var instance = await MainSystem.Instance.AddressableManager.InstantiateAsync("EnemyDeathSkull", cancellationToken: _token);
            instance.transform.position = transform.parent.position;

            Destroy(transform.parent.gameObject);
        });
    }

    private void PlayAttackParticle()
    {
        _attackParticle.Play();
    }

    #endregion
}
