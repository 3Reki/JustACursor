using System;
using Bosses.Instructions.Patterns.Drones;
using LD;
using UnityEditor;
using UnityEngine;

namespace Editor.Patterns.Drones
{
    [CustomEditor(typeof(Pat_Dr_Cross))]
    public class Pat_Dr_CrossEditor : UnityEditor.Editor
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
                    Handles.DrawLine(start, end, 3);
                }
                
                Handles.DrawSolidDisc(Vector3.Lerp(start, end, (i % 3 + 0.5f) / 3), Vector3.back, (end - start).magnitude * 0.04f);
                Handles.ArrowHandleCap(
                    0,
                    Vector3.Lerp(start, end, (i % 3 + 0.5f) / 3),
                    rotation,
                    1,
                    EventType.Repaint
                );
            }
        }

        private static void GetPosition(out Vector2 start, out Vector2 end, out Quaternion rotation, int index, Vector3 topLeft, Vector3 bottomRight)
        {
            Vector2 bottomLeft = new Vector2(topLeft.x, bottomRight.y);
            Vector2 topRight = new Vector2(bottomRight.x, topLeft.y);
            switch (index / 3)
            {
                case 0:
                    start = Vector2.Lerp(topLeft, bottomLeft, .2f);
                    end = Vector2.Lerp(topLeft, topRight, .2f);
                    rotation = Quaternion.LookRotation(new Vector3(1, -1), Vector3.back);
                    break;
                case 1:
                    start = Vector2.Lerp(topRight, topLeft, .2f);
                    end = Vector2.Lerp(topRight, bottomRight, .2f);
                    rotation = Quaternion.LookRotation(new Vector3(-1, -1), Vector3.back);
                    break;
                case 2:
                    start = Vector2.Lerp(bottomRight, topRight, .2f);
                    end = Vector2.Lerp(bottomRight, bottomLeft, .2f);
                    rotation = Quaternion.LookRotation(new Vector3(-1, 1), Vector3.back);
                    break;
                case 3:
                    start = Vector2.Lerp(bottomLeft, bottomRight, .2f);
                    end = Vector2.Lerp(bottomLeft, topLeft, .2f);
                    rotation = Quaternion.LookRotation(new Vector3(1, 1), Vector3.back);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}