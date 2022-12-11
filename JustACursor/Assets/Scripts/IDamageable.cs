using UnityEngine;

public interface IDamageable
{
    public void Damage(BulletPro.Bullet bullet, Vector3 hitPoint);
}