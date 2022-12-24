using LD;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(LevelManager))]
    public class LevelManagerEditor : UnityEditor.Editor
    {
        private LevelManager editedLevel;

        private void OnEnable()
        {
            editedLevel = target as LevelManager;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            DrawDefaultInspector();
            editedLevel.NbMaxFloorShown = EditorGUILayout.IntSlider("NbMaxFloorShown", editedLevel.NbMaxFloorShown, 1, editedLevel.Floors.Count);

            GUILayout.Space(10);
            GUILayout.Label("Editor Only", EditorStyles.boldLabel);
            if (GUILayout.Button("Get Components"))
            {
                if (Application.isPlaying)
                {
                    Debug.LogWarning("Editor Only !");
                    return;
                }
                editedLevel.GetComponents();
            }

            if (GUILayout.Button("Setup Floors"))
            {
                if (Application.isPlaying)
                {
                    Debug.LogWarning("Editor Only !");
                    return;
                }
                editedLevel.SetupFloors();
            }
                
            if (GUILayout.Button("Reset All (for editing)"))
            {
                if (Application.isPlaying)
                {
                    Debug.LogWarning("Editor Only !");
                    return;
                }
                editedLevel.ResetAll();
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
                editedLevel.GoToNextFloor();
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}