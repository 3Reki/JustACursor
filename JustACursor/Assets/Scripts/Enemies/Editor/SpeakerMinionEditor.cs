using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Enemies.Editor
{
    [CustomEditor(typeof(SpeakerMinion))]
    public class SpeakerMinionEditor : UnityEditor.Editor
    {
        private SpeakerMinion minion;
    
        private void OnEnable() {
            minion = target as SpeakerMinion;
        }
        
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            minion.IsActiveAtStart = EditorGUILayout.Toggle("Is Active At Start", minion.IsActiveAtStart);

            if (minion.IsActiveAtStart)
            {
                DrawDefaultInspector();
            }

            serializedObject.ApplyModifiedProperties();
            
            if (GUI.changed)
            {
                EditorUtility.SetDirty(minion);
                EditorSceneManager.MarkSceneDirty(minion.gameObject.scene);
            }
        }
    }
}