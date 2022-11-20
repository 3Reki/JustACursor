using BulletPro;
using UnityEngine;

namespace Bosses.Patterns
{
    [CreateAssetMenu(fileName = "Pat_CenterPattern", menuName = "Just A Cursor/Pattern/Center")]
    public class Pat_Center : Pattern
    {
        [SerializeField] private EmitterProfile[] emitterProfiles;
        
        public override void Play()
        {
            boss.GoToCenter();
            boss.transform.rotation = Quaternion.Euler(0, 0, 180);
            for (int i = 0; i < emitterProfiles.Length; i++)
            {
                boss.bulletEmitter[i].SwitchProfile(emitterProfiles[i]);
            }
        }

        public override void Stop()
        {
            
            for (int i = 0; i < emitterProfiles.Length; i++)
            {
                boss.bulletEmitter[i].Stop(); 
            }
        }
    }
}