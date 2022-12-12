using Enemies;
using UnityEngine;

namespace Bosses.Instructions.Patterns
{
    [CreateAssetMenu(fileName = "Pat_Laser", menuName = "Just A Cursor/Pattern/Laser Pattern", order = 0)]
    public class Pat_Laser : Pattern<ILaserHolder>
    {
        [SerializeField] private float previewDuration;
        [SerializeField] private float laserWidth;
        [SerializeField] private float laserLength;
        
        public override void Play(ILaserHolder entity)
        {
            base.Play(entity);
            
            entity.StartFire(previewDuration, patternDuration - previewDuration, laserWidth, laserLength);
        }

        public override void Stop()
        {
            linkedEntity.CeaseFire();
        }
    }
}