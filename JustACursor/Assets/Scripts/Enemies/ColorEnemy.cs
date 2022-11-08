using Interfaces;
using UnityEngine;

namespace Enemies
{
    public class ColorEnemy : MonoBehaviour, IDamageable
    {
        [SerializeField] private Health health;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Gradient colorDamage;

        public void ChangeColor()
        {
            spriteRenderer.color = colorDamage.Evaluate(1-(float)health.GetCurrentHealth() / health.GetMaxHealth());
        }
    
        public void Damage(BulletPro.Bullet bullet, Vector3 hitPoint)
        {
            health.LoseHealth(bullet.moduleParameters.GetInt("Damage"));
        }

        public void Die()
        {
            Destroy(gameObject);
        }
    }
}
