using UnityEngine;

namespace Levels
{
    public interface IDamageable
    {
        public void Damage(BulletPro.Bullet bullet, Vector3 hitPoint);
    }
}

