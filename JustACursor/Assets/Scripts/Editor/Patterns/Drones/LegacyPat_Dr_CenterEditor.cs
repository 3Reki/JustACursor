using LD;
using LegacyBosses.Patterns.Drones;
using UnityEditor;
using UnityEngine;

namespace Editor.Patterns.Drones
{
    [CustomEditor(typeof(Pat_Dr_Center))]
    public class LegacyPat_Dr_CenterEditor : UnityEditor.Editor
    {
        private SerializedProperty m_distanceToCenter, m_flipFormation;
        
        private void OnEnable()
        {
            m_distanceToCenter = serializedObject.FindProperty("distanceToCenter");
            m_flipFormation = serializedObject.FindProperty("flipFormation");
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

            Vector2 center;
            {
                var roomSO = new SerializedObject(room);
                var topLeftCorner = (Transform) roomSO.FindProperty("roomTopLeftCorner").objectReferenceValue;
                var bottomRightCorner = (Transform) roomSO.FindProperty("roomBottomRightCorner").objectReferenceValue;

                if (!topLeftCorner || !bottomRightCorner)
                {
                    return;
                }

                Vector2 topLeftPos = topLeftCorner.position;
                Vector2 bottomRightPos = bottomRightCorner.position;
                center = new Vector2((bottomRightPos.x + topLeftPos.x) * 0.5f,
                    (topLeftPos.y + bottomRightPos.y) * 0.5f);
            }
            
            float distanceToCenter = m_distanceToCenter.floatValue;
            DrawWirePattern(center, distanceToCenter);
            Handles.color = Color.white;
            distanceToCenter = Handles.ScaleSlider(distanceToCenter, center, Vector3.right, Quaternion.identity,
                HandleUtility.GetHandleSize(center), 0.01f);

            if (GUI.changed)
            {
                // apply changes back to target component
                m_distanceToCenter.floatValue = Mathf.Abs(distanceToCenter);
                serializedObject.ApplyModifiedProperties();
            }
        }

        private void DrawWirePattern(Vector3 roomCenter, float distanceToCenter)
        {
            Handles.color = Color.red;

            bool flipFormation = m_flipFormation.boolValue;

            float cos;
            float sin;
            {
                float angle = (flipFormation ? 0f : 0.5f) / 12f * Mathf.PI * 2f;
                cos = Mathf.Cos(angle);
                sin = Mathf.Sin(angle);
            }
            
            for (int i = 0; i < 12; i++)
            {
                float nextAngle = (i + 1 + (flipFormation ? 0 : 0.5f)) / 12 * Mathf.PI * 2;
                float nextCos = Mathf.Cos(nextAngle);
                float nextSin = Mathf.Sin(nextAngle);

                Handles.DrawSolidDisc(roomCenter + distanceToCenter * new Vector3(cos, sin), Vector3.back, distanceToCenter * 0.04f);
                Handles.DrawLine(roomCenter + distanceToCenter * new Vector3(cos, sin), roomCenter + distanceToCenter * new Vector3(nextCos, nextSin), 2);
                
                cos = nextCos;
                sin = nextSin;
            }
        }
    }
}