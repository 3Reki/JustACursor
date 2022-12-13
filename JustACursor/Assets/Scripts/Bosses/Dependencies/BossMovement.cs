using System;
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

        public void GoToBorder(Room.Half border, float moveDuration)
        {
            MoveTo(GetBorder(border), moveDuration);
        }

        public void RotateTowardsPlayer()
        {
            Vector3 dir = player.transform.position - transform.position;
            dir.Normalize();
            float zRotation = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, zRotation - 90);
        }

        public Vector2 GetCorner(int cornerIndex)
        {
            return coneFirePoints[cornerIndex].position;
        }

        public Vector2 GetBorder(Room.Half border)
        {
            return border switch
            {
                Room.Half.North => Vector2.Lerp(coneFirePoints[0].position, coneFirePoints[1].position, 0.5f),
                Room.Half.East => Vector2.Lerp(coneFirePoints[1].position, coneFirePoints[3].position, 0.5f),
                Room.Half.South => Vector2.Lerp(coneFirePoints[2].position, coneFirePoints[3].position, 0.5f),
                Room.Half.West => Vector2.Lerp(coneFirePoints[0].position, coneFirePoints[2].position, 0.5f),
                _ => throw new ArgumentOutOfRangeException(nameof(border), border, null)
            };
        }

        private void MoveTo(Vector3 dest, float moveDuration)
        {
            bossTransform.DOMove(dest, moveDuration);
        }
    }
}