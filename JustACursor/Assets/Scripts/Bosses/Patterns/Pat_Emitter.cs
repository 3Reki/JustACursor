using BulletPro;
using UnityEngine;

namespace Bosses.Patterns
{
    [CreateAssetMenu(fileName = "Pat_Emitter", menuName = "Just A Cursor/Pattern/Emitter Pattern", order = 0)]
    public class Pat_Emitter : Pattern
    {
        [SerializeField] private EmitterProfile[] emitterProfiles = new EmitterProfile[3];
        
        public override Pattern Play(Boss boss)
        {
            base.Play(boss);
            
            for (int i = 0; i < emitterProfiles.Length; i++)
            {
                linkedBoss.bulletEmitter[i].SwitchProfile(emitterProfiles[i]);
            }

            return this;
        }

        public override Pattern Stop()
        {
            for (int i = 0; i < emitterProfiles.Length; i++)
            {
                linkedBoss.bulletEmitter[i].Stop(); 
            }
            return base.Stop();
        }
    }
}