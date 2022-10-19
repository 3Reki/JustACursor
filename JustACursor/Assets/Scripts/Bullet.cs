using Player;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private new Rigidbody2D rigidbody;
    [SerializeField] private float bulletSpeed;

    public void Shoot(Vector2 direction)
    {
        //TODO : Observer pattern to make bulletSpeed dynamic
        Vector2 force = direction * (bulletSpeed * PlayerEnergy.GameSpeed);
        rigidbody.AddForce(force, ForceMode2D.Impulse);
    }

    private void Update() {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Pooler.instance.DePop(Pooler.Key.Bullet, gameObject);
    }
}
