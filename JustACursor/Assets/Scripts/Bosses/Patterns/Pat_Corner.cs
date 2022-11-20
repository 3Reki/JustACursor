using System.Collections;
using BulletPro;
using UnityEngine;

namespace Bosses.Patterns
{
    [CreateAssetMenu(fileName = "Pat_CornerPattern", menuName = "Just A Cursor/Pattern/Corner")]
    public class Pat_Corner : Pattern
    {
        [SerializeField] private EmitterProfile[] emitterProfiles;

        private IEnumerator playEnumerator;

        public override void Play()
        {
            boss.GoToRandomCorner();
            for (int i = 0; i < emitterProfiles.Length; i++)
            {
                boss.bulletEmitter[i].SwitchProfile(emitterProfiles[i]);
            }

            playEnumerator = RotationCoroutine();
            boss.StartCoroutine(playEnumerator);
        }

        public override void Stop()
        {
            if (playEnumerator != null)
                boss.StopCoroutine(playEnumerator);
            
            for (int i = 0; i < emitterProfiles.Length; i++)
            {
                boss.bulletEmitter[i].Stop(); 
            }
        }

        private IEnumerator RotationCoroutine()
        {
            float timer = length;

            while (timer > 0)
            {
                boss.RotateTowardsPlayer();
                yield return null;
                timer -= Time.deltaTime;
            }
        }
    }
}