using Bosses.Instructions;
using Bosses.Instructions.Patterns;
using UnityEditor;
using Object = UnityEngine.Object;

namespace Editor
{
    [CustomEditor(typeof(Pat_Composite))]
    public class Pat_CompositeEditor : UnityEditor.Editor
    {
        private SerializedProperty m_patterns;
        private float patternDuration;

        private void OnEnable()
        {
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
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.FloatField("Pattern Duration", patternDuration);
            EditorGUI.EndDisabledGroup();
            
            serializedObject.ApplyModifiedProperties();
        }
    }
}