using CameraScripts;
using Player;
using UnityEngine;

namespace LD
{
    public class LevelHandler : MonoBehaviour
    {
        [SerializeField] private PlayerRespawn player;
        [SerializeField] private Floor[] floors;
        [SerializeField, Range(0.2f,0.05f)] private float scaleDecreaseValue;
        [SerializeField, Range(0.1f,2f)] private float scaleDuration;
        
        private int currentFloorIndex;
        
        public delegate void OnEndFloor();
        public static OnEndFloor onEndFloor;

        private void OnEnable()
        {
            onEndFloor += GoToNextFloor;
        }

        private void OnDisable()
        {
            onEndFloor -= GoToNextFloor;
        }

        private void Start()
        {
            player.SetCheckpoint(floors[currentFloorIndex].StartPoint);
        }

        public void GetComponents()
        {
            floors = GetComponentsInChildren<Floor>(true);
            player = FindObjectOfType<PlayerRespawn>();
        }

        public void GoToNextFloor()
        {
            //Disable current floor
            floors[currentFloorIndex].gameObject.SetActive(false);
            currentFloorIndex++;
            
            if (currentFloorIndex >= floors.Length) return;
            
            //Next floor
            CameraController.instance.enabled = true;
            player.transform.SetPositionAndRotation(floors[currentFloorIndex].StartPoint.position, floors[currentFloorIndex].StartPoint.rotation);
            player.SetCheckpoint(floors[currentFloorIndex].StartPoint);
            UpdateFloors();
        }

        public void SetupFloors()
        {
            for (int i = 0; i < floors.Length; i++)
            {
                Floor floor = floors[i];
                floor.GetComponents();
                
                float scaleValue = Mathf.Clamp(1 - scaleDecreaseValue * i,0.1f,1);
                floor.Scale(scaleValue,scaleDuration);
                
                floor.SetAllTriggerState(i == 0);

                string sortingLayerName = (i == 0) ? "CurrentFloor" : "OtherFloor";
                floor.SetSortingLayer(sortingLayerName,i);
            }
            
            Debug.Log("Floors have been setup successfully!");
        }

        public void ResetAll()
        {
            for (int i = 0; i < floors.Length; i++)
            {
                Floor floor = floors[i];
                
                floor.Scale(1,scaleDuration);
                floor.SetAllTriggerState(true);
                floor.SetSortingLayer("CurrentFloor",i);
            }
            
            Debug.Log("Floors have been reset successfully!");
        }

        private void UpdateFloors()
        {
            floors[currentFloorIndex].SetAllTriggerState(true);
            
            for (int i = currentFloorIndex; i < floors.Length; i++)
            {
                Floor floor = floors[i];
                
                float scaleValue = Mathf.Clamp(1 - scaleDecreaseValue * i,0.1f,1);
                floor.Scale(scaleValue,scaleDuration);

                string sortingLayerName = (i == currentFloorIndex) ? "CurrentFloor" : "OtherFloor";
                floor.SetSortingLayer(sortingLayerName,i);
            }
        }
    }
}
