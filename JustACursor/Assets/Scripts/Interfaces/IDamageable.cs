using UnityEngine;

namespace Interfaces
{
    public interface IDamageable
    {
        public void Damage(BulletPro.Bullet bullet, Vector3 hitPoint);
    }
}

