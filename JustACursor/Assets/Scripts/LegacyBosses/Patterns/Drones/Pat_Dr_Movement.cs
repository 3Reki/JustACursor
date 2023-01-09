using UnityEngine;

namespace LegacyBosses.Patterns.Drones
{
    public abstract class Pat_Dr_Movement : Pattern<BossSound>
    {
        private PositionAndRotation[] initials;
        private PositionAndRotation[] destinations;

        public override void Play(BossSound entity)
        {
            base.Play(entity);

            initials = new PositionAndRotation[linkedEntity.droneCount];
            destinations = new PositionAndRotation[linkedEntity.droneCount];

            for (int i = 0; i < linkedEntity.droneCount; i++)
            {
                Transform droneTransform = linkedEntity.GetDrone(i).transform;
                initials[i] = new PositionAndRotation()
                {
                    position = droneTransform.position,
                    rotation = droneTransform.rotation
                };
            }
            
            if (patternDuration == 0f)
            {
                Debug.LogWarning("Movement duration shouldn't be equal to 0, this may cause errors or unexpected behaviours.");
            }
        }

        public override void Update()
        {
            base.Update();

            float currentLerpTime = 1 - currentPatternTime / patternDuration;

            for (int i = 0; i < linkedEntity.droneCount; i++)
            {
                Vector3 pos = Vector3.Lerp(initials[i].position, destinations[i].position, currentLerpTime);
                Quaternion rot = Quaternion.Lerp(initials[i].rotation, destinations[i].rotation, currentLerpTime);
                linkedEntity.GetDrone(i).SetPositionAndRotation(pos, rot);
            }
        }

        public override void Stop()
        {
        }
        
        protected void SetTarget(int droneIndex, Vector3 position, Quaternion rotation)
        {
            destinations[droneIndex].position = position;
            destinations[droneIndex].rotation = rotation;
        }
    }

    internal struct PositionAndRotation
    {
        public Vector3 position;
        public Quaternion rotation;
    }
}