using Levels;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(LevelHandler))]
    public class LevelHandlerEditor : UnityEditor.Editor
    {
        private LevelHandler editedLevelHandler;

        private void OnEnable()
        {
            editedLevelHandler = target as LevelHandler;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            DrawDefaultInspector();

            GUILayout.Space(10);
            GUILayout.Label("Editor Only", EditorStyles.boldLabel);
            if (GUILayout.Button("Get Components"))
            {
                if (Application.isPlaying)
                {
                    Debug.LogWarning("Editor Only !");
                    return;
                }
                editedLevelHandler.GetComponents();
            }

            if (GUILayout.Button("Setup Floors"))
            {
                if (Application.isPlaying)
                {
                    Debug.LogWarning("Editor Only !");
                    return;
                }
                editedLevelHandler.SetupFloors();
            }
                
            if (GUILayout.Button("Reset All (for editing)"))
            {
                if (Application.isPlaying)
                {
                    Debug.LogWarning("Editor Only !");
                    return;
                }
                editedLevelHandler.ResetAll();
            }
            
            GUILayout.Space(10);
            GUILayout.Label("Runtime Only", EditorStyles.boldLabel);
            if (GUILayout.Button("Go to Next Floor"))
            {
                if (!Application.isPlaying)
                {
                    Debug.LogWarning("Runtime Only !");
                    return;
                }
                editedLevelHandler.GoToNextFloor();
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}