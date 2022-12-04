using Levels;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(LevelHandler))]
    public class LevelHandlerEditor : UnityEditor.Editor
    {
        private LevelHandler editedLevelHandler;
        private float scaleDecreaseValue;

        private void OnEnable()
        {
            editedLevelHandler = target as LevelHandler;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            DrawDefaultInspector();

            //scaleDecreaseValue = EditorGUILayout.FloatField("Scale Decrease Per Floor :",scaleDecreaseValue);

            if (GUILayout.Button("Setup Floors"))
            {
                editedLevelHandler.OrderFloors();
                editedLevelHandler.ScaleFloors();
                
                /*List<Floor> _floors = editedLevelHandler.Floors;
                
                TilemapRenderer[] _tilemapRenderers = _floors[0].GetComponentsInChildren<TilemapRenderer>();
                SpriteRenderer[] _spriteRenderers = _floors[0].GetComponentsInChildren<SpriteRenderer>();
                foreach (TilemapRenderer _renderer in _tilemapRenderers)
                {
                    _renderer.sortingLayerName = "CurrentFloor";
                    _renderer.sortingOrder = 0;
                }
                        
                foreach (SpriteRenderer _renderer in _spriteRenderers)
                {
                    _renderer.sortingLayerName = "CurrentFloor";
                    _renderer.sortingOrder = -1;
                }
                
                for (int i = 1; i < _floors.Count; i++)
                {
                    float _newScale = Mathf.Clamp(1 - scaleDecreaseValue * i,0.1f,1);
                    _floors[i].transform.localScale = new Vector3(_newScale, _newScale, _newScale);
                    _tilemapRenderers = _floors[i].GetComponentsInChildren<TilemapRenderer>();
                    _spriteRenderers = _floors[i].GetComponentsInChildren<SpriteRenderer>();
                    
                    foreach (TilemapRenderer _renderer in _tilemapRenderers)
                    {
                        _renderer.sortingLayerName = "OtherFloor";
                        _renderer.sortingOrder = -i;
                    }
                        
                    foreach (SpriteRenderer _renderer in _spriteRenderers)
                    {
                        _renderer.sortingLayerName = "OtherFloor";
                        _renderer.sortingOrder = -i-1;
                    }
                }*/
            }

            if (Application.isPlaying)
            {
                if (GUILayout.Button("Go to next floor"))
                {
                    editedLevelHandler.GoToNextFloor();
                }
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}