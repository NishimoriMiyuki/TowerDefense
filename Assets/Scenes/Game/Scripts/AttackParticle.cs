using UnityEngine;

public class AttackParticle : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem _particleSystem;

    private int _attackPower;

    private bool _damageAlreadyGiven = false;
    public bool DamageAlreadyGiven { get => _damageAlreadyGiven; set => _damageAlreadyGiven = value; }

    private void OnParticleCollision(GameObject other)
    {
        if (_damageAlreadyGiven)
        {
            return;
        }

        IDamageable damageableObject = other.GetComponentInChildren<IDamageable>();

        if (damageableObject == null)
        {
            Debug.LogError("Idamageableが付いていない");
            return;
        }

        damageableObject.TakeDamage(_attackPower);

        Debug.Log("Attackぱわー = " + _attackPower);

        _damageAlreadyGiven = true;
    }

    public void Play()
    {
        _particleSystem.Play();
    }

    public void SetAttackPower(int attackPower)
    {
        _attackPower = attackPower;
    }
}
