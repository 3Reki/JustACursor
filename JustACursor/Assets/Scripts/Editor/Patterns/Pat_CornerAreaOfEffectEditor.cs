using Bosses.Patterns;
using LD;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;

namespace Editor.Patterns
{
    [CustomEditor(typeof(Pat_CornerAreaOfEffect))]
    public class Pat_CornerAreaOfEffectEditor : Pat_AreaOfEffectEditor
    {
        private SerializedProperty m_corner, m_sizeMultiplier;
        
        protected override void OnEnable()
        {
            base.OnEnable();
            m_corner = serializedObject.FindProperty("corner");
            m_sizeMultiplier = serializedObject.FindProperty("sizeMultiplier");
            SceneView.duringSceneGui += OnSceneGUI;
        }

        private void OnDisable()
        {
            SceneView.duringSceneGui -= OnSceneGUI;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EditorGUILayout.PropertyField(m_corner);
            EditorGUILayout.PropertyField(m_sizeMultiplier);

            serializedObject.ApplyModifiedProperties();
        }

        private void OnSceneGUI(SceneView sceneView)
        {
            serializedObject.Update();
            
            var room = FindObjectOfType<Room>();
            if (!room)
            {
                return;
            }

            Vector2 position = room.GetCorner(m_corner.GetEnumValue<Room.Quarter>());

            if (position == Vector2.negativeInfinity)
            {
                return;
            }
            
            Handles.color = new Color(1, 0, 0, 0.4f);
            Handles.DrawSolidDisc(position, Vector3.back, m_sizeMultiplier.floatValue * 0.5f);
            Handles.color = Color.white;

            float size = Handles.ScaleSlider(m_sizeMultiplier.floatValue, position, Vector3.right, Quaternion.identity,
                HandleUtility.GetHandleSize(position), 0.01f);

            if (GUI.changed)
            {
                // apply changes back to target component
                m_sizeMultiplier.floatValue = Mathf.Abs(size);
                serializedObject.ApplyModifiedProperties();
            }
        }
    }
}