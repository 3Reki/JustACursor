using System.Threading.Tasks;
using BulletPro;
using UnityEngine;

namespace Bosses.Patterns
{
    [CreateAssetMenu(fileName = "Pat_CenterPattern", menuName = "Just A Cursor/Pattern/Center")]
    public class Pat_Center : Pattern
    {
        [SerializeField] private EmitterProfile[] emitterProfiles;
        [SerializeField] private float movementDuration;
        
        public override async void Play()
        {
            boss.mover.GoToCenter(movementDuration);
            await Task.Delay((int) (movementDuration * 1000));
            
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