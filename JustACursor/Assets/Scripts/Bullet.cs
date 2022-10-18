using Player;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private new Rigidbody2D rigidbody;
    [SerializeField] private Transform myTransform;
    [SerializeField] private float bulletSpeed;

    public void Shoot()
    {
        rigidbody.AddForce(myTransform.up * bulletSpeed, ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Pooler.instance.DePop("Bullet", gameObject);
    }
}