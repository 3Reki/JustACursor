using UnityEngine;

public class ColorEnemy : MonoBehaviour, IDamageable
{
    [SerializeField] private Health health;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Gradient colorDamage;

    public void ChangeColor()
    {
        spriteRenderer.color = colorDamage.Evaluate(1-(float)health.GetCurrentHealth() / health.GetMaxHealth());
    }
    
    public void Damage(int amount)
    {
        health.LoseHealth(amount);
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}
