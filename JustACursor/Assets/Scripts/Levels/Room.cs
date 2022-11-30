using System;
using System.ComponentModel;
using UnityEngine;

namespace Levels
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
        
        public enum Corner
        {
            Any = -1,
            TopLeft = 0,
            TopRight = 1,
            BottomLeft = 2,
            BottomRight = 3
        }
    }
}