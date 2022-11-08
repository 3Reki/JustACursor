using BulletPro;
using UnityEngine;

namespace Player
{
    public class PlayerShoot : MonoBehaviour
    {
        [SerializeField] private BulletEmitter emitter;

        public void StartShooting()
        {
            emitter.Play();
        }

        public void StopShooting()
        {
            emitter.Stop();
        }
    }
}