using UnityEngine;

namespace LegacyBosses.Patterns.Drones
{
    public class Pat_Dr_Center : Pat_Dr_Movement
    {
        [Min(0)]
        [SerializeField] private float distanceToCenter;
        [SerializeField] private bool flipFormation;

        public override void Play(BossSound entity)
        {
            base.Play(entity);

            int droneCount = linkedEntity.droneCount;

            if (droneCount != 12)
            {
                Debug.Log(
                    "Pat_Dr_Center is meant to be used with 12 drones. Other drone count may result in unexpected behaviours");
            }

            Vector3 roomCenter = linkedEntity.mover.Room.middleCenter;
            for (int i = 0; i < droneCount; i++)
            {
                // offset if flipFormation is false to match Julia's ATK-R1 pattern, else match ATK-R1bis
                float angle = (i + (flipFormation ? 0 : 0.5f)) / 12 * Mathf.PI * 2;
                float cos = Mathf.Cos(angle);
                float sin = Mathf.Sin(angle);

                float droneZRot;
                if (flipFormation)
                {
                    droneZRot = (i % 3) switch
                    {
                        0 => -90 + 90 * (i / 3),
                        1 => -45 + 90 * (i / 3),
                        _ => -45 + 90 * (i / 3)
                    };
                }
                else
                {
                    droneZRot = (i % 3) switch
                    {
                        0 => -90 + 90 * (i / 3),
                        1 => angle * Mathf.Rad2Deg - 90,
                        _ => 90 * (i / 3)
                    };
                }

                SetTarget(i, roomCenter + distanceToCenter * new Vector3(cos, sin), 
                    Quaternion.Euler(0, 0, droneZRot));
            }
        }
    }
}