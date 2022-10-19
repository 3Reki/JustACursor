using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private new Rigidbody2D rigidbody;
    [SerializeField] private Transform myTransform;
    [SerializeField] private float bulletSpeed;

    public void Shoot(Vector2 direction)
    {
        Vector2 force = direction * bulletSpeed;
        rigidbody.AddForce(force, ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Pooler.instance.DePop(Pooler.Key.Bullet, gameObject);
    }
}
