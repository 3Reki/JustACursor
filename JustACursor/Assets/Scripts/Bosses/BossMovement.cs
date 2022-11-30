using DG.Tweening;
using Levels;
using Player;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Bosses
{
    public class BossMovement : MonoBehaviour
    {
        public Room room;
        
        [SerializeField] private Transform bossTransform;
        [SerializeField] private PlayerController player;
        [SerializeField] private Transform[] coneFirePoints;

        public void GoToCenter(float moveDuration)
        {
            MoveTo(room.centerPosition, moveDuration);
        }

        public void GoToRandomCorner(float moveDuration)
        {
            MoveTo(GetRandomCorner(), moveDuration);
        }
        
        public void RotateTowardsPlayer()
        {
            Vector3 dir = player.transform.position - transform.position;
            dir.Normalize();
            float zRotation = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, zRotation - 90);
        }

        private Vector3 GetRandomCorner()
        {
            return coneFirePoints[Random.Range(0, coneFirePoints.Length)].position;
        }

        private void MoveTo(Vector3 dest, float moveDuration)
        {
            bossTransform.DOMove(dest, moveDuration);
        }
    }
}