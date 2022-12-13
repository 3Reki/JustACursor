using System;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace LD
{
    public static class RoomExtensionMethods
    {
        public static Vector2 GetCorner(this Room room, Room.Quarter quarter)
        {
            return GetCorner(room, (Room.Corner) quarter);
        }
        
#if UNITY_EDITOR
        public static Vector2 GetCorner(this Room room, Room.Corner corner)
        {
            if (EditorApplication.isPlaying)
            {
                return corner switch
                {
                    Room.Corner.NorthWest => room.topLeft,
                    Room.Corner.NorthEast => room.topRight,
                    Room.Corner.SouthWest => room.bottomLeft,
                    Room.Corner.SouthEast => room.bottomRight,
                    _ => throw new ArgumentOutOfRangeException(nameof(corner), corner, null)
                };
            }
            
            Vector3 topLeftPos;
            Vector3 bottomRightPos;
            {
                var roomSO = new SerializedObject(room);
                var topLeftCorner = (Transform) roomSO.FindProperty("roomTopLeftCorner").objectReferenceValue;
                var bottomRightCorner = (Transform) roomSO.FindProperty("roomBottomRightCorner").objectReferenceValue;

                if (!topLeftCorner || !bottomRightCorner)
                {
                    return Vector2.negativeInfinity;
                }

                topLeftPos = topLeftCorner.position;
                bottomRightPos = bottomRightCorner.position;
            }
            
            return corner switch
            {
                Room.Corner.NorthWest => topLeftPos,
                Room.Corner.NorthEast => new Vector2(bottomRightPos.x, topLeftPos.y),
                Room.Corner.SouthWest => new Vector2(topLeftPos.x, bottomRightPos.y),
                Room.Corner.SouthEast => bottomRightPos,
                _ => throw new ArgumentOutOfRangeException(nameof(corner), corner, null)
            };
        }
#else
        public static Vector2 GetCorner(this Room room, Room.Corner corner)
        {
            return corner switch
            {
                Room.Corner.NorthWest => room.topLeft,
                Room.Corner.NorthEast => room.topRight,
                Room.Corner.SouthWest => room.bottomLeft,
                Room.Corner.SouthEast => room.bottomRight,
                _ => throw new ArgumentOutOfRangeException(nameof(corner), corner, null)
            };
        }
#endif
        
    }
}