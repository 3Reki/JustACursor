using UnityEngine;

namespace LegacyBosses.Patterns.Drones
{
    public class Pat_Dr_ShootLaser : Pattern<BossSound>
    {
        [SerializeField] private float previewDuration;
        [SerializeField] private float laserWidth;
        [SerializeField] private float laserLength;

        public override void Play(BossSound entity)
        {
            base.Play(entity);

            int droneCount = linkedEntity.droneCount;

            for (int i = 0; i < droneCount; i++)
            {
                linkedEntity.GetDrone(i)
                    .StartFire(previewDuration, patternDuration - previewDuration, laserWidth, laserLength);
            }
        }

        public override void Stop()
        {
            int droneCount = linkedEntity.droneCount;
            
            for (int i = 0; i < droneCount; i++)
            {
                linkedEntity.GetDrone(i).CeaseFire();
            }
        }
    }
}