using LegacyBosses.Patterns;
using UnityEditor;
using UnityEngine;

namespace Editor.Patterns
{
    [CustomEditor(typeof(Pat_AreaOfEffect<>))]
    public class LegacyPat_AreaOfEffectEditor : UnityEditor.Editor
    {
        private SerializedProperty m_patternDuration, m_aoePrefab, m_previewDuration;

        protected virtual void OnEnable()
        {
            m_patternDuration = serializedObject.FindProperty("patternDuration");
            m_aoePrefab = serializedObject.FindProperty("aoePrefab");
            m_previewDuration = serializedObject.FindProperty("previewDuration");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(m_aoePrefab, new GUIContent("Area Of Effect Prefab"));
            float aoeDuration = EditorGUILayout.FloatField(
                new GUIContent("Area Of Effect Lifespan", "The duration of the area of effect in seconds"),
                m_patternDuration.floatValue - m_previewDuration.floatValue);

            if (aoeDuration < 0)
            {
                aoeDuration = 0;
            }

            EditorGUILayout.PropertyField(m_previewDuration,
                new GUIContent(m_previewDuration.displayName,
                    "The duration of the area of effect's preview in seconds"));

            m_patternDuration.floatValue = aoeDuration + m_previewDuration.floatValue;

            serializedObject.ApplyModifiedProperties();
        }
    }
}