using DG.Tweening;
using LD;
using Player;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Bosses.Dependencies
{
    public class BossMovement : MonoBehaviour
    {
        public Room room;

        [SerializeField] private Transform bossTransform;
        [SerializeField] private PlayerController player;
        [SerializeField] private Transform[] coneFirePoints;

        public void GoToCenter(float moveDuration)
        {
            MoveTo(room.middleCenter, moveDuration);
        }

        public void GoToCorner(Room.Corner corner, float moveDuration)
        {
            MoveTo(GetCorner(corner == Room.Corner.Any ? Random.Range(0, coneFirePoints.Length) : (int) corner),
                moveDuration);
        }

        public void RotateTowardsPlayer()
        {
            Vector3 dir = player.transform.position - transform.position;
            dir.Normalize();
            float zRotation = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, zRotation - 90);
        }

        private Vector3 GetCorner(int cornerIndex)
        {
            return coneFirePoints[cornerIndex].position;
        }

        private void MoveTo(Vector3 dest, float moveDuration)
        {
            bossTransform.DOMove(dest, moveDuration);
        }
    }
}