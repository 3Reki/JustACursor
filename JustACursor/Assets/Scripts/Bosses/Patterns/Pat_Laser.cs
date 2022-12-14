using Enemies;
using UnityEngine;

namespace Bosses.Patterns
{
    [CreateAssetMenu(fileName = "Pat_Laser", menuName = "Just A Cursor/Pattern/Laser Pattern", order = 0)]
    public class Pat_Laser : Pattern<ILaserHolder>
    {
        [SerializeField] private float previewDuration;
        [SerializeField] private float laserWidth;
        [SerializeField] private float laserLength;
        [SerializeField] private bool isEndless;
        
        public override void Play(ILaserHolder entity)
        {
            base.Play(entity);
            
            entity.StartFire(previewDuration, isEndless ? float.PositiveInfinity : patternDuration - previewDuration, laserWidth, laserLength);
        }

        public override void Update()
        {
            if (!isEndless)
            {
                base.Update();
            }
        }

        public override void Stop()
        {
            base.Stop();
            linkedEntity.CeaseFire();
        }
    }
}