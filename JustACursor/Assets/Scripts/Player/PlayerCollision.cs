using System.Collections;
using Interfaces;
using ScriptableObjects;
using UnityEngine;

namespace Player {
    public class PlayerCollision : MonoBehaviour, IDamageable
    {
        public bool isInvincible;
        
        [SerializeField] private PlayerController playerController;
        [SerializeField] private Health health;

        private PlayerData data => playerController.data;

        public void Damage(BulletPro.Bullet bullet, Vector3 hitPoint)
        {
            if (isInvincible) return;
            
            health.LoseHealth(bullet.moduleParameters.GetInt("Damage"));
            StartCoroutine(Invincibility());
        }

        private IEnumerator Invincibility()
        {
            isInvincible = true;
            yield return new WaitForSeconds(data.invinciblityTime);
            isInvincible = false;
        }
    }
}

