using Bosses.Instructions;
using UnityEditor;
using Object = UnityEngine.Object;

namespace Editor
{
    [CustomEditor(typeof(InstructionSimple<>))]
    public class InstructionSimpleEditor : UnityEditor.Editor
    {
        private SerializedProperty m_resolver, m_pattern;
        private float patternDuration;

        private void OnEnable()
        {
            m_resolver = serializedObject.FindProperty("resolver");
            m_pattern = serializedObject.FindProperty("pattern");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            
            patternDuration = 0;

            {
                Object objRef = m_pattern.objectReferenceValue;
                
                if (objRef != null)
                    patternDuration = new SerializedObject(objRef).FindProperty("patternDuration").floatValue;
            }

            EditorGUILayout.PropertyField(m_pattern);
            EditorGUILayout.PropertyField(m_resolver);
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.FloatField("Pattern Duration", patternDuration);
            EditorGUI.EndDisabledGroup();
            
            serializedObject.ApplyModifiedProperties();
        }
    }
    
    [CustomEditor(typeof(InstructionSimpleBoss))]
    public class InstructionSimpleBossEditor : InstructionSimpleEditor {}
    
    [CustomEditor(typeof(InstructionSimpleDrones))]
    public class InstructionSimpleDronesEditor : InstructionSimpleEditor {}
}