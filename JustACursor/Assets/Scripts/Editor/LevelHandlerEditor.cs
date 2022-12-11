using LD;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(LevelHandler))]
    public class LevelHandlerEditor : UnityEditor.Editor
    {
        private LevelHandler editedLH;

        private void OnEnable()
        {
            editedLH = target as LevelHandler;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            DrawDefaultInspector();
            editedLH.NbMaxFloorShown = EditorGUILayout.IntSlider("NbMaxFloorShown", editedLH.NbMaxFloorShown, 1, editedLH.Floors.Count);

            GUILayout.Space(10);
            GUILayout.Label("Editor Only", EditorStyles.boldLabel);
            if (GUILayout.Button("Get Components"))
            {
                if (Application.isPlaying)
                {
                    Debug.LogWarning("Editor Only !");
                    return;
                }
                editedLH.GetComponents();
            }

            if (GUILayout.Button("Setup Floors"))
            {
                if (Application.isPlaying)
                {
                    Debug.LogWarning("Editor Only !");
                    return;
                }
                editedLH.SetupFloors();
            }
                
            if (GUILayout.Button("Reset All (for editing)"))
            {
                if (Application.isPlaying)
                {
                    Debug.LogWarning("Editor Only !");
                    return;
                }
                editedLH.ResetAll();
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
                editedLH.GoToNextFloor();
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}