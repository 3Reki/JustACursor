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
            if (GUILayout.Button("Setup LD"))
            {
                if (Application.isPlaying)
                {
                    Debug.LogWarning("Editor Only !");
                    return;
                }
                editedLevel.SetupLD();
            }
                
            if (GUILayout.Button("Reset Layers"))
            {
                if (Application.isPlaying)
                {
                    Debug.LogWarning("Editor Only !");
                    return;
                }
                editedLevel.ResetLayers();
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