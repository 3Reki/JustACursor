using UnityEngine;

namespace LD
{
    public class Obstacle : MonoBehaviour, IDamageable
    {
        [SerializeField] private Health health;
        [SerializeField] private int healthPoint;

        private void Start()
        {
            health.Init(healthPoint);
        }
        
        public void Damage(BulletPro.Bullet bullet, Vector3 hitPoint)
        {
            health.LoseHealth(bullet.moduleParameters.GetInt("Damage"));
            if (health.CurrentHealth == 0) Destroy(gameObject);
        }
    }
}
