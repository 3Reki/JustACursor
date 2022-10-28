using System.Collections;
using Player;
using ScriptableObjects;
using UnityEngine;

public class PlayerCollision : MonoBehaviour, IDamageable
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private Health health;

    private PlayerData data => playerController.data;
    private bool isInvincible;

    public void Damage(int amount)
    {
        health.LoseHealth(amount);
        StartCoroutine(Invincibility());
    }

    private IEnumerator Invincibility()
    {
        isInvincible = true;
        yield return new WaitForSeconds(data.invinciblityTime);
        isInvincible = false;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (isInvincible) return;
        
        if (col.gameObject.TryGetComponent(out ColorEnemy enemy))
        {
            Damage(1);
        }
    }
}
