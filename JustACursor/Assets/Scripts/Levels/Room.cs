using System;
using System.ComponentModel;
using UnityEngine;

namespace Levels
{
    public class Room : MonoBehaviour
    {
        public Vector2 topLeft => corners[0];
        public Vector2 topCenter => new(middleCenter.x, corners[0].y);
        public Vector2 topRight => corners[1];
        public Vector2 middleLeft => new(corners[0].x, middleCenter.y);
        public Vector2 middleCenter { get; private set; }
        public Vector2 middleRight => new(corners[1].x, middleCenter.y);
        public Vector2 bottomLeft => corners[2];
        public Vector2 bottomCenter => new(middleCenter.x, corners[2].y);
        public Vector2 bottomRight => corners[3];
        
        [SerializeField] private Transform roomTopLeftCorner;
        [SerializeField] private Transform roomBottomRightCorner;
        
        private Vector2[] corners;

        private void Awake()
        {
            Vector2 topLeftPos = roomTopLeftCorner.position;
            Vector2 bottomRightPos = roomBottomRightCorner.position;
            corners = new[] {topLeftPos, new Vector2(bottomRightPos.x, topLeftPos.y), new Vector2(topLeftPos.x, bottomRightPos.y), bottomRightPos};
            middleCenter = new Vector2((bottomRightPos.x + topLeftPos.x) * 0.5f,
                (topLeftPos.y + bottomRightPos.y) * 0.5f);
        }

        public float DistanceToCenter(Vector2 position) => Vector2.Distance(middleCenter, position);

        public float MinDistanceToCorners(Vector2 position)
        {
            float min = float.PositiveInfinity;
            foreach (Vector2 corner in corners)
            {
                float cornerDistance = Vector2.Distance(corner, position);
                if (cornerDistance < min)
                {
                    min = cornerDistance;
                }
            }

            return min;
        }

        public float DistanceToCorner(Vector2 position, Corner corner)
        {
            if (corner == Corner.Any)
            {
                throw new InvalidEnumArgumentException(
                    "Corner.Any is not a valid value. Use MinDistanceToCorners instead.");
            }
            
            return Vector2.Distance(corners[(int) corner], position);
        }

        public bool IsInsideQuarter(Vector2 position, Quarter quarter)
        {
            return quarter switch
            {
                Quarter.NorthWest => position.x <= middleCenter.x && position.y > middleCenter.y,
                Quarter.NorthEast => position.x > middleCenter.x && position.y >= middleCenter.y,
                Quarter.SouthWest => position.x < middleCenter.x && position.y <= middleCenter.y,
                Quarter.SouthEast => position.x >= middleCenter.x && position.y < middleCenter.y,
                _ => throw new ArgumentOutOfRangeException(nameof(quarter), quarter, null)
            };
        }
        
        public bool IsInsideHalf(Vector2 position, Half half)
        {
            return half switch
            {
                Half.North => position.y > middleCenter.y,
                Half.East => position.x > middleCenter.x,
                Half.South => position.y <= middleCenter.y,
                Half.West => position.x <= middleCenter.x,
                _ => throw new ArgumentOutOfRangeException(nameof(half), half, null)
            };
        }
        
        public enum Corner
        {
            Any = -1,
            NorthWest = 0,
            NorthEast = 1,
            SouthWest = 2,
            SouthEast = 3
        }

        public enum Quarter
        {
            NorthWest = 0,
            NorthEast = 1,
            SouthWest = 2,
            SouthEast = 3
        }
        
        public enum Half
        {
            North = 0,
            East = 1,
            South = 2,
            West = 3
        }
    }
}