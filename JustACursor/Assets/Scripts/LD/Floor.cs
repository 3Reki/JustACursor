using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace LD
{
    public class Floor : MonoBehaviour
    {
        public enum FloorState { All, TilemapOnly, Nothing }
        
        [field:SerializeField] public Checkpoint StartPoint { get; private set; }

        [SerializeField] private TilemapCollider2D walls;
        [SerializeField] private Renderer[] renderers;
        [SerializeField] private List<GameObject> floorTilemaps;
        [SerializeField] private List<GameObject> floorElements = new();

        public void GetComponents()
        {
            StartPoint = GetComponentInChildren<Checkpoint>(true);
            walls = GetComponentInChildren<TilemapCollider2D>(true);
            renderers = GetComponentsInChildren<Renderer>(true);
            
            floorTilemaps.Clear();
            floorElements.Clear();
            foreach (Transform child in transform)
            {
                if (child.GetComponent<TilemapRenderer>()) floorTilemaps.Add(child.gameObject);
                else floorElements.Add(child.gameObject);
            }
        }
        
        public void Scale(float scaleValue)
        {
            transform.localScale = new Vector3(scaleValue, scaleValue, scaleValue);
        }

        public void SetFloorState(FloorState state)
        {
            switch (state)
            {
                case FloorState.Nothing:
                    walls.enabled = false;
                    UpdateGOVisibility(floorTilemaps, false);
                    UpdateGOVisibility(floorElements, false);
                    break;
                case FloorState.All:
                    walls.enabled = true;
                    UpdateGOVisibility(floorTilemaps, true);
                    UpdateGOVisibility(floorElements, true);
                    break;
                case FloorState.TilemapOnly:
                    walls.enabled = false;
                    UpdateGOVisibility(floorTilemaps, true);
                    UpdateGOVisibility(floorElements, false);
                    break;
            }
        }

        private void UpdateGOVisibility(List<GameObject> list, bool state)
        {
            foreach (GameObject go in list)
            {
                go.SetActive(state);
            }
        }

        public void SetSortingLayer(string sortingLayerName, int sortingOrder)
        {
            foreach (Renderer rend in renderers)
            {
                rend.sortingLayerName = sortingLayerName;
                if (rend.gameObject.TryGetComponent(out FloorSort fs))
                {
                    rend.sortingOrder = fs.RenderPlan == Plan.Foreground ? sortingOrder+1 : sortingOrder-1;
                }
                else
                {
                    rend.sortingOrder = sortingOrder;
                }
            }
        }
    }
}