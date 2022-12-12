using System;
using Bosses.Instructions.Patterns.Drones;
using LD;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;

namespace Editor.Patterns.Drones
{
    [CustomEditor(typeof(Pat_Dr_DiagonalHalfRoom))]
    public class Pat_Dr_DiagonalHalfRoomEditor : UnityEditor.Editor
    {
        private SerializedProperty m_startingCorner;
        
        private void OnEnable()
        {
            m_startingCorner = serializedObject.FindProperty("startingCorner");
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

            Vector3 topLeftPos;
            Vector3 bottomRightPos;
            {
                var roomSO = new SerializedObject(room);
                var topLeftCorner = (Transform) roomSO.FindProperty("roomTopLeftCorner").objectReferenceValue;
                var bottomRightCorner = (Transform) roomSO.FindProperty("roomBottomRightCorner").objectReferenceValue;

                if (!topLeftCorner || !bottomRightCorner)
                {
                    return;
                }

                topLeftPos = topLeftCorner.position;
                bottomRightPos = bottomRightCorner.position;
            }
            
            DrawWirePattern(topLeftPos, bottomRightPos);
        }

        private void DrawWirePattern(Vector3 topLeftPos, Vector3 bottomRightPos)
        {
            Handles.color = Color.red;
            
            GetLine(out Vector3 lineStart, out Vector3 lineEnd, out Quaternion rotation, topLeftPos, bottomRightPos);
            Handles.DrawLine(lineStart, lineEnd, 3);

            for (int i = 0; i < 12; i++)
            {
                Handles.DrawSolidDisc(Vector3.Lerp(lineStart, lineEnd, (i + 0.5f) / 12), Vector3.back, (lineEnd - lineStart).magnitude * 0.008f);
                Handles.ArrowHandleCap(
                    0,
                    Vector3.Lerp(lineStart, lineEnd, (i + 0.5f) / 12),
                    rotation,
                    1,
                    EventType.Repaint
                );
            }
        }

        private void GetLine(out Vector3 lineStart, out Vector3 lineEnd, out Quaternion rotation, Vector3 topLeftPos, Vector3 bottomRightPos)
        {
            switch (m_startingCorner.GetEnumValue<Room.Quarter>())
            {
                case Room.Quarter.NorthWest:
                    lineStart = topLeftPos;
                    lineEnd = bottomRightPos;
                    rotation = Quaternion.LookRotation(Vector2.one, Vector3.back);
                    break;
                case Room.Quarter.NorthEast:
                    lineStart = new Vector3(bottomRightPos.x, topLeftPos.y);
                    lineEnd = new Vector3(topLeftPos.x, bottomRightPos.y);
                    rotation = Quaternion.LookRotation(new Vector3(1, -1), Vector3.back);
                    break;
                case Room.Quarter.SouthWest:
                    lineStart = new Vector3(bottomRightPos.x, topLeftPos.y);
                    lineEnd = new Vector3(topLeftPos.x, bottomRightPos.y);
                    rotation = Quaternion.LookRotation(new Vector3(-1, 1), Vector3.back);
                    break;
                case Room.Quarter.SouthEast:
                    lineStart = topLeftPos;
                    lineEnd = bottomRightPos;
                    rotation = Quaternion.LookRotation(-Vector2.one, Vector3.back);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}