using System.ComponentModel;
using UnityEngine;

namespace LD
{
    public class Room : MonoBehaviour
    {
        public Vector2 centerPosition { get; private set; }
        
        [SerializeField] private Transform roomTopLeftCorner;
        [SerializeField] private Transform roomBottomRightCorner;
        
        private Vector2[] corners;

        private void Awake()
        {
            Vector2 topLeftPos = roomTopLeftCorner.position;
            Vector2 bottomRightPos = roomBottomRightCorner.position;
            corners = new[] {topLeftPos, new Vector2(bottomRightPos.x, topLeftPos.y), new Vector2(topLeftPos.x, bottomRightPos.y), bottomRightPos};
            centerPosition = new Vector2((bottomRightPos.x + topLeftPos.x) * 0.5f,
                (topLeftPos.y + bottomRightPos.y) * 0.5f);
        }

        public float DistanceToCenter(Vector2 position) => Vector2.Distance(centerPosition, position);

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
                Quarter.NorthWest => position.x <= centerPosition.x && position.y > centerPosition.y,
                Quarter.NorthEast => position.x > centerPosition.x && position.y >= centerPosition.y,
                Quarter.SouthWest => position.x < centerPosition.x && position.y <= centerPosition.y,
                Quarter.SouthEast => position.x >= centerPosition.x && position.y < centerPosition.y,
                _ => throw new ArgumentOutOfRangeException(nameof(quarter), quarter, null)
            };
        }
        
        public bool IsInsideHalf(Vector2 position, Half half)
        {
            return half switch
            {
                Half.North => position.y > centerPosition.y,
                Half.East => position.x > centerPosition.x,
                Half.South => position.y <= centerPosition.y,
                Half.West => position.x <= centerPosition.x,
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