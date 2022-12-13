using System;
using Bosses.Patterns.Drones;
using LD;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;

namespace Editor.Patterns.Drones
{
    [CustomEditor(typeof(Pat_Dr_HalfRoom))]
    public class Pat_Dr_HalfRoomEditor : UnityEditor.Editor
    {
        private SerializedProperty m_half, m_alternateDirectionMode;
        
        private void OnEnable()
        {
            m_half = serializedObject.FindProperty("half");
            m_alternateDirectionMode = serializedObject.FindProperty("alternateDirectionMode");
            SceneView.duringSceneGui += OnSceneGUI;
        }

        private void OnDisable()
        {
            SceneView.duringSceneGui -= OnSceneGUI;
        }

        private void OnSceneGUI(SceneView sceneView)
        {
            serializedObject.Update();
            var room = FindObjectOfType<Room>();
            if (!room)
            {
                return;
            }

            Vector3 center;
            Vector3 halfSize;
            {
                var roomSO = new SerializedObject(room);
                var topLeftCorner = (Transform) roomSO.FindProperty("roomTopLeftCorner").objectReferenceValue;
                var bottomRightCorner = (Transform) roomSO.FindProperty("roomBottomRightCorner").objectReferenceValue;

                if (!topLeftCorner || !bottomRightCorner)
                {
                    return;
                }

                Vector3 topLeftPos = topLeftCorner.position;
                Vector3 bottomRightPos = bottomRightCorner.position;
                halfSize = new Vector3((bottomRightPos.x - topLeftPos.x) * 0.5f,
                    (topLeftPos.y - bottomRightPos.y) * 0.5f);
                center = new Vector3(topLeftPos.x, bottomRightPos.y) + halfSize;
            }
            
            DrawWirePattern(center, halfSize);
        }

        private void DrawWirePattern(Vector3 roomCenter, Vector3 halfSize)
        {
            Handles.color = Color.red;
            
            GetLine(out Vector3 lineStart, out Vector3 lineEnd, roomCenter, halfSize);
            Handles.DrawLine(lineStart, lineEnd, 3);

            var altDirMode = m_alternateDirectionMode.GetEnumValue<Pat_Dr_HalfRoom.AlternateDirectionMode>();
            var half = m_half.GetEnumValue<Room.Half>();

            for (int i = 0; i < 12; i++)
            {
                Handles.DrawSolidDisc(Vector3.Lerp(lineStart, lineEnd, (i + 0.5f) / 12), Vector3.back, (lineEnd - lineStart).magnitude * 0.008f);
                Handles.ArrowHandleCap(
                    0,
                    Vector3.Lerp(lineStart, lineEnd, (i + 0.5f) / 12),
                    GetRotation(i, 12, altDirMode, half),
                    1,
                    EventType.Repaint
                );
            }
        }

        private void GetLine(out Vector3 lineStart, out Vector3 lineEnd, Vector3 roomCenter, Vector3 halfSize)
        {
            switch (m_half.GetEnumValue<Room.Half>())
            {
                case Room.Half.North:
                    lineStart = new Vector3(roomCenter.x - halfSize.x, roomCenter.y);
                    lineEnd = new Vector3(roomCenter.x + halfSize.x, roomCenter.y);
                    break;
                case Room.Half.East:
                    lineStart = new Vector3(roomCenter.x, roomCenter.y + halfSize.y);
                    lineEnd = new Vector3(roomCenter.x, roomCenter.y - halfSize.y);
                    break;
                case Room.Half.South:
                    lineStart = new Vector3(roomCenter.x - halfSize.x, roomCenter.y);
                    lineEnd = new Vector3(roomCenter.x + halfSize.x, roomCenter.y);
                    break;
                case Room.Half.West:
                    lineStart = new Vector3(roomCenter.x, roomCenter.y + halfSize.y);
                    lineEnd = new Vector3(roomCenter.x, roomCenter.y - halfSize.y);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        private Quaternion GetRotation(int index, int totalCount, Pat_Dr_HalfRoom.AlternateDirectionMode alternateDirectionMode, Room.Half half)
        {
            return (index * (int) alternateDirectionMode / totalCount) % 2 == 0 ? GetSimpleRotation(half) : GetInversedRotation(half);
        }
        
        private Quaternion GetSimpleRotation(Room.Half half)
        {
            return half switch
            {
                Room.Half.North => Quaternion.LookRotation(Vector3.up, Vector3.back),
                Room.Half.East => Quaternion.LookRotation(Vector3.right, Vector3.back),
                Room.Half.South => Quaternion.LookRotation(Vector3.down, Vector3.back),
                Room.Half.West => Quaternion.LookRotation(Vector3.left, Vector3.back),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
        
        private Quaternion GetInversedRotation(Room.Half half)
        {
            return half switch
            {
                Room.Half.North => Quaternion.LookRotation(Vector3.down, Vector3.back),
                Room.Half.East => Quaternion.LookRotation(Vector3.left, Vector3.back),
                Room.Half.South => Quaternion.LookRotation(Vector3.up, Vector3.back),
                Room.Half.West => Quaternion.LookRotation(Vector3.right, Vector3.back),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}