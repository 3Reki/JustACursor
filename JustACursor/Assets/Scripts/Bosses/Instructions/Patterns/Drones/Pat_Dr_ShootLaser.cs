using System.Collections;
using UnityEngine;

namespace Bosses.Instructions.Patterns.Drones
{
    [CreateAssetMenu(fileName = "Pat_Dr_Shoot", menuName = "Just A Cursor/Pattern/Drones/Shoot Pattern", order = 0)]
    public class Pat_Dr_ShootLaser : Pattern<BossSound>
    {
        [SerializeField] private float previewDuration;
        [SerializeField] private float laserWidth;
        [SerializeField] private float laserLength;
        
        private IEnumerator[] shootEnumerator;
        
        public override void Play(BossSound boss)
        {
            base.Play(boss);

            int droneCount = linkedEntity.droneCount;

            shootEnumerator = new IEnumerator[droneCount];
            
            for (int i = 0; i < droneCount; i++)
            {
                shootEnumerator[i] = linkedEntity.GetDrone(i)
                    .ShootOneShot(previewDuration, patternDuration, laserWidth, laserLength);
                linkedEntity.GetDrone(i).StartCoroutine(shootEnumerator[i]);
            }
        }

        public override void Stop()
        {
            int droneCount = linkedEntity.droneCount;
            
            for (int i = 0; i < droneCount; i++)
            {
                linkedEntity.GetDrone(i).StopCoroutine(shootEnumerator[i]);
            }
        }
    }
}