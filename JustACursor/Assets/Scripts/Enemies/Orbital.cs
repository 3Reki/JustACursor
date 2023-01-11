using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MyBox;
using UnityEngine;

namespace Enemies
{
    public class Orbital : MonoBehaviour
    {
        [SerializeField] private Health health;
        [SerializeField] private List<OrbitalTurret> turrets;
        [SerializeField] private Transform turretParent;

        [Separator("GD")]
        [Header("Health")]
        [SerializeField] private int maxHealth;
        [SerializeField] private int turretMaxHealth;

        [Header("Turrets")]
        [SerializeField] private bool clockwiseRotation;
        [SerializeField] private float turretRotationSpeed;
        [SerializeField] private float timeBeforeFirstFire;
        [SerializeField] private float turretShootCooldown;
    
        [Header("Level")]
        [SerializeField, Range(1,4)] private int level;

        private bool turretShield;
        private float angle;

        private void Awake()
        {
            if (level < 2) turrets[2].gameObject.SetActive(false);

            if (level < 3)
            {
                turrets[1].gameObject.SetActive(false);
                turrets[3].gameObject.SetActive(false);
            }

            if (level == 4) turretShield = true;
        }

        private void Start()
        {
            health.Init(maxHealth);
        
            foreach (OrbitalTurret turret in turrets)
            {
                if (!turret.gameObject.activeSelf) break;
                turret.Init(turretMaxHealth);
            }
        
            StartCoroutine(FirstFireDelay());
        }

        private void Update()
        {
            if (!clockwiseRotation) angle += Time.deltaTime * Energy.GameSpeed * turretRotationSpeed;
            else angle += Time.deltaTime * Energy.GameSpeed * turretRotationSpeed;
            turretParent.rotation = Quaternion.Euler(0,0,angle);
        }
    
        private IEnumerator FirstFireDelay()
        {
            yield return new WaitForSeconds(timeBeforeFirstFire);
            StartCoroutine(FireLoop());
        }
    
        private IEnumerator FireLoop()
        {
            foreach (OrbitalTurret turret in turrets)
            {
                if (!turret.gameObject.activeSelf) continue;
                turret.Fire();
            }
        
            float timer = turretShootCooldown;
            while (timer > 0)
            {
                yield return null;
                timer -= Time.deltaTime * Energy.GameSpeed;
            }
            
            StartCoroutine(FireLoop());
        }

        public void RemoveFromTurretList(OrbitalTurret turret)
        {
            turrets.Remove(turret);
            if (turrets.Count == 0) turretShield = false;
        }
    
        public void Damage(BulletPro.Bullet bullet, Vector3 hitPoint)
        {
            if (turretShield) return;
            health.LoseHealth(bullet.moduleParameters.GetInt("Damage"));
        }

        public void Die()
        {
            Destroy(gameObject);
        }
    }
}
