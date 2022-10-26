using Player;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private new Rigidbody2D rigidbody;
    [SerializeField] private float bulletSpeed;

    private void OnEnable()
    {
        PlayerEnergy.onGameSpeedUpdate += UpdateSpeed;
    }

    private void OnDisable()
    {
        PlayerEnergy.onGameSpeedUpdate -= UpdateSpeed;
    }

    public void Shoot(Vector2 direction)
    {
        rigidbody.velocity = direction * (bulletSpeed * PlayerEnergy.GameSpeed);
    }

    private void UpdateSpeed()
    {
        rigidbody.velocity = rigidbody.velocity.normalized * (bulletSpeed * PlayerEnergy.GameSpeed);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out IDamageable entity))
        {
            entity.Damage(1);
        }
        
        Pooler.instance.DePop(Pooler.Key.Bullet, gameObject);
    }
}
