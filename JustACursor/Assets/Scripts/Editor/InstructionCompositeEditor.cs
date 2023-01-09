using LegacyBosses.Instructions;
using UnityEditor;
using Object = UnityEngine.Object;

namespace Editor
{
    [CustomEditor(typeof(InstructionComposite<>))]
    public class InstructionCompositeEditor : UnityEditor.Editor
    {
        private SerializedProperty m_resolver, m_patterns;
        private float patternDuration;

        private void OnEnable()
        {
            m_resolver = serializedObject.FindProperty("resolver");
            m_patterns = serializedObject.FindProperty("patterns");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            
            patternDuration = 0;
            for (int i = 0; i < m_patterns.arraySize; i++)
            {
                Object objRef = m_patterns.GetArrayElementAtIndex(i).objectReferenceValue;
                
                if (objRef == null)
                    continue;

                float currentPatternDuration = new SerializedObject(objRef).FindProperty("patternDuration").floatValue;
                if (currentPatternDuration > patternDuration)
                {
                    patternDuration = currentPatternDuration;
                }
            }

            EditorGUILayout.PropertyField(m_patterns);
            EditorGUILayout.PropertyField(m_resolver);
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.FloatField("Pattern Duration", patternDuration);
            EditorGUI.EndDisabledGroup();
            
            serializedObject.ApplyModifiedProperties();
        }
    }
    
    [CustomEditor(typeof(InstructionCompositeBoss))]
    [CanEditMultipleObjects]
    public class InstructionCompositeBossEditor : InstructionCompositeEditor {}
    
    [CustomEditor(typeof(InstructionCompositeDrones))]
    [CanEditMultipleObjects]
    public class InstructionCompositeDronesEditor : InstructionCompositeEditor {}
}