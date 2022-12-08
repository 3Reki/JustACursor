using Bosses.Dependencies;
using Bosses.Patterns.Drones;
using UnityEngine;

namespace Bosses
{
    public class BossSound : Boss
    {
        public int droneCount => drones.Length;
        [Space(15)]
        [Header("=== Sound Boss ===")]
        [SerializeField] private TempDrone[] drones = new TempDrone[12];

        [SerializeField] private Pat_Dr_HalfRoom pat;

        [SerializeField] private Resolver<BossSound> dronesResolver;
        
        protected override void Start()
        {
            base.Start();
            
            pat.Play(this);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            var otherHealth = other.gameObject.GetComponent<Health>();
            if (otherHealth)
            {
                otherHealth.LoseHealth(1);
            }
        }

        public TempDrone GetDrone(int i)
        {
            return drones[i];
        }
    }
}