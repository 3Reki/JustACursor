using DG.Tweening;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace LD
{
    public class Floor : MonoBehaviour
    {
        [field:SerializeField] public Transform StartPoint;

        [SerializeField] private Renderer[] renderers;
        [SerializeField] private Collider2D[] colliders;

        public void GetComponents()
        {
            renderers = GetComponentsInChildren<Renderer>(true);
            colliders = GetComponentsInChildren<Collider2D>(true);
        }
        
        public void Scale(float scaleValue, float scaleDuration)
        {
            if (Application.isPlaying)
            {
                transform.DOScale(new Vector3(scaleValue, scaleValue, scaleValue), scaleDuration);
            }
            else
            {
                transform.localScale = new Vector3(scaleValue, scaleValue, scaleValue);
            }
        } 
        
        public void SetAllTriggerState(bool state)
        {
            foreach (Collider2D col in colliders)
            {
                col.enabled = state;
            }
        }

        public void SetSortingLayer(string sortingLayerName, int sortingOrder)
        {
            foreach (Renderer rend in renderers)
            {
                rend.sortingLayerName = sortingLayerName;
                rend.sortingOrder = rend is TilemapRenderer ? sortingOrder : sortingOrder-1;
            }
        }
    }
}