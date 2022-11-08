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
        rigidbody.velocity = direction * (bulletSpeed * Energy.GameSpeed);
    }

    private void UpdateSpeed()
    {
        rigidbody.velocity = rigidbody.velocity.normalized * (bulletSpeed * Energy.GameSpeed);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Pooler.instance.DePop(Pooler.Key.Bullet, gameObject);
    }
}
