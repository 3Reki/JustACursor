using BulletPro;
using UnityEngine;

namespace LegacyBosses.Patterns
{
    [CreateAssetMenu(fileName = "Pat_Emitter", menuName = "Just A Cursor/Pattern/Emitter Pattern", order = 0)]
    public class Pat_Emitter : Pattern<Boss>
    {
        [SerializeField, Tooltip("Max 3 profiles")] private EmitterProfile[] emitterProfiles;
        
        public override void Play(Boss entity)
        {
            base.Play(entity);
            
            for (int i = 0; i < emitterProfiles.Length; i++)
            {
                linkedEntity.bulletEmitter[i].SwitchProfile(emitterProfiles[i]);
                linkedEntity.bulletEmitter[i].Play();
            }
        }

        public override void Stop()
        {
            for (int i = 0; i < emitterProfiles.Length; i++)
            {
                linkedEntity.bulletEmitter[i].Stop();
            }
        }
    }
}