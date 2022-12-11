using System.Collections.Generic;
using BulletPro;
using LD;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(Floor))]
    public class FloorEditor : UnityEditor.Editor
    {
        private Floor editedFloor;
        
        private bool drawReceiverMode;
        private List<Vector2> selectedPoints = new();
        
        private Camera mainCam;

        private void OnEnable()
        {
            editedFloor = target as Floor;
            mainCam = Camera.main;
        }
        
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            
            /*GUILayout.Label("Floor Editor", EditorStyles.boldLabel);
            drawReceiverMode = EditorGUILayout.Toggle("Draw Receivers Mode", drawReceiverMode);
            if (drawReceiverMode)
            {
                if (GUILayout.Button($"Create Receivers ({selectedPoints.Count})"))
                {
                    //CreateReceivers();
                }

                if (GUILayout.Button("Reset"))
                {
                    selectedPoints.Clear();
                }
            }
            
            GUILayout.Space(10);*/
            
            DrawDefaultInspector();
            serializedObject.ApplyModifiedProperties();
        }

        /*private void OnSceneGUI()
        {
            if (!drawReceiverMode) return;
            HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
            Event e = Event.current;
            if (e.type == EventType.MouseUp)
            {
                AddSelectedPoint(e.mousePosition);
                DrawReceiver();
            }
        }

        private void AddSelectedPoint(Vector3 pos)
        {
            pos = mainCam.ScreenToWorldPoint(pos);
            selectedPoints.Add(pos);
        }

        private void DrawReceiver()
        {
            if (selectedPoints.Count < 2) return;

            Vector2 startPos = selectedPoints[0];
            Vector2 endPos = selectedPoints[1];
            
            Debug.Log(startPos);
            Debug.Log(endPos);
            
            Vector2 currentVector = Vector2.up - startPos;
            Vector2 wantedVector = endPos - startPos;
            
            GameObject newGO = Instantiate(new GameObject(), editedFloor.CompositeBulletReceiver.transform);
            newGO.name = "Hitbox";

            BulletReceiver br = newGO.AddComponent<BulletReceiver>();
            br.transform.position = editedFloor.CompositeBulletReceiver.transform.position;
            br.colliderType = BulletReceiverType.Line;
            br.hitboxSize = (startPos - endPos).magnitude;

            br.hitboxOffset = startPos;

            float angle = Vector2.SignedAngle(currentVector, wantedVector);
            //br.transform.rotation = Quaternion.Euler(0,0,angle);
            
            selectedPoints.RemoveAt(0);
        }*/
    }
}