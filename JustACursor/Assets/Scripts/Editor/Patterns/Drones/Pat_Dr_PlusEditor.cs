using System;
using Bosses.Patterns.Drones;
using LD;
using UnityEditor;
using UnityEngine;

namespace Editor.Patterns.Drones
{
    [CustomEditor(typeof(Pat_Dr_Plus))]
    public class Pat_Dr_PlusEditor : UnityEditor.Editor
    {
        private void OnEnable()
        {
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

            Vector2 start = Vector2.zero;
            Vector2 end = Vector2.zero;
            Quaternion rotation = Quaternion.identity;
            for (int i = 0; i < 12; i++)
            {
                if (i % 3 == 0)
                {
                    GetPosition(out start, out end, out rotation, i, topLeftPos, bottomRightPos);
                    Handles.DrawLine(Vector3.Lerp(start, end, 5f / 12), Vector3.Lerp(start, end, 7f / 12), 3);
                }

                Handles.DrawSolidDisc(Vector3.Lerp(start, end, (i % 3 + 5f) / 12), Vector3.back,
                    (end - start).magnitude * 0.008f);
                Handles.ArrowHandleCap(
                    0,
                    Vector3.Lerp(start, end, (i % 3 + 5f) / 12),
                    rotation,
                    1,
                    EventType.Repaint
                );
            }
        }

        private static void GetPosition(out Vector2 start, out Vector2 end, out Quaternion rotation, int index,
            Vector3 topLeft, Vector3 bottomRight)
        {
            Vector2 bottomLeft = new Vector2(topLeft.x, bottomRight.y);
            Vector2 topRight = new Vector2(bottomRight.x, topLeft.y);
            switch (index / 3)
            {
                case 0:
                    start = topLeft;
                    end = topRight;
                    rotation = Quaternion.LookRotation(Vector3.down, Vector3.back);
                    break;
                case 1:
                    start = topRight;
                    end = bottomRight;
                    rotation = Quaternion.LookRotation(Vector3.left, Vector3.back);
                    break;
                case 2:
                    start = bottomRight;
                    end = bottomLeft;
                    rotation = Quaternion.LookRotation(Vector3.up, Vector3.back);
                    break;
                case 3:
                    start = bottomLeft;
                    end = topLeft;
                    rotation = Quaternion.LookRotation(Vector3.right, Vector3.back);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}