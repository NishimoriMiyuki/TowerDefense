using Cysharp.Threading.Tasks;

public interface IDamageable
{
    UniTaskVoid TakeDamage(int damage);
}
