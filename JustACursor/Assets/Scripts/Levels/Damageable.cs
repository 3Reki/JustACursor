using TMPro;
using UnityEngine;

namespace Levels
{
    public class Damageable : MonoBehaviour
    {
        [SerializeField] private int startingHP;
        [SerializeField] private TMP_Text hp; 
        private int curHealth;
        private bool isAlive = true;

        private void Start()
        {
            curHealth = startingHP;
            hp.text = $"HP: {curHealth}";
        }

        public void Hurt(BulletPro.Bullet bullet, Vector3 hitPoint)
        {
            if (!isAlive) return;
            curHealth -= bullet.moduleParameters.GetInt("Damage");

            UpdateLifebar();

            if (curHealth <= 0) Die(); ; 
        }

        private void UpdateLifebar()
        {
            hp.text = $"HP: {curHealth}";  
        }

        private void Die()
        {
            isAlive = false; 
            Debug.Log("Player is dead");
            transform.root.gameObject.SetActive(false);
        }
    }
}
