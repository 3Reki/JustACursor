using System.Collections.Generic;
using CameraScripts;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Levels
{
    public class LevelHandler : MonoBehaviour
    {
        public List<Floor> Floors;
        [SerializeField] private float scaleDecreaseValue;
        [SerializeField] private Transform player;

        private int currentFloorIndex;
        
        public void GoToNextFloor()
        {
            Floors[currentFloorIndex].gameObject.SetActive(false);
            currentFloorIndex++;
            
            player.transform.SetPositionAndRotation(Floors[currentFloorIndex].StartPoint.position, Floors[currentFloorIndex].StartPoint.rotation);
            
            OrderFloors();
            ScaleFloors();
        }

        public void OrderFloors()
        {
            for (int i = currentFloorIndex; i < Floors.Count; i++)
            {
                TilemapRenderer[] tilemapRenderers = Floors[i].GetComponentsInChildren<TilemapRenderer>();
                SpriteRenderer[] spriteRenderers = Floors[i].GetComponentsInChildren<SpriteRenderer>();
                    
                foreach (TilemapRenderer tmRenderer in tilemapRenderers)
                {
                    tmRenderer.sortingLayerName = (i == currentFloorIndex) ? "CurrentFloor" : "OtherFloor";
                    tmRenderer.sortingOrder = -i;
                }
                        
                foreach (SpriteRenderer tmRenderer in spriteRenderers)
                {
                    tmRenderer.sortingLayerName = (i == currentFloorIndex) ? "CurrentFloor" : "OtherFloor";
                    tmRenderer.sortingOrder = -i-1;
                }
            }
        }

        public void ScaleFloors()
        {
            for (int i = currentFloorIndex; i < Floors.Count; i++)
            {
                float newScale = Mathf.Clamp(1 - scaleDecreaseValue * i,0.1f,1);
                if (Application.isPlaying)
                {
                    Floors[i].transform.DOScale(new Vector3(newScale,newScale,newScale),1.5f);
                }
                else
                {
                    Floors[i].transform.localScale = new Vector3(newScale, newScale, newScale);
                }
            }
        }
        
        public delegate void OnEndFloor();
        public static OnEndFloor onEndFloor; 
    }
}
