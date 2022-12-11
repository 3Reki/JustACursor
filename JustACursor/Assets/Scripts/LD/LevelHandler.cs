using System;
using System.Collections.Generic;
using CameraScripts;
using Player;
using UnityEngine;

namespace LD
{
    public class LevelHandler : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private PlayerRespawn player;
        [field:SerializeField] public List<Floor> Floors { get; private set; }

        [Header("Parameters")]
        [SerializeField, Range(0.05f,0.2f)] private float scaleDecreaseValue;
        [HideInInspector] public int NbMaxFloorShown;
        
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
            player.SetCheckpoint(Floors[0].StartPoint);
        }

        public void GoToNextFloor()
        {
            //Disable current floor
            Floors[0].gameObject.SetActive(false);
            Floors.RemoveAt(0);
            
            if (Floors.Count == 0) return;
            
            //Next floor
            CameraController.instance.enabled = true;
            player.transform.SetPositionAndRotation(Floors[0].StartPoint.position, Floors[0].StartPoint.rotation);
            player.SetCheckpoint(Floors[0].StartPoint);
            UpdateFloors();
        }
        
        private void UpdateFloors()
        {
            NbMaxFloorShown = Math.Min(NbMaxFloorShown, Floors.Count);
            for (int i = 0; i < NbMaxFloorShown; i++)
            {
                Floor floor = Floors[i];
                float scaleValue = Mathf.Clamp(1 - scaleDecreaseValue * i,0.1f,1);
                floor.Scale(scaleValue);
                
                string sortingLayerName = (i == 0) ? "CurrentFloor" : "OtherFloor";
                floor.SetSortingLayer(sortingLayerName,-i);

                floor.SetFloorState(i == 0 ? Floor.FloorState.All : Floor.FloorState.TilemapOnly);
            }

            for (int i = NbMaxFloorShown; i < Floors.Count; i++)
            {
                Floors[i].SetFloorState(Floor.FloorState.Nothing);
            }
        }
        
        /*
         * Methods for LevelHandlerEditor
         */
        
        public void GetComponents()
        {
            Floors = new List<Floor>(GetComponentsInChildren<Floor>(true));
            player = FindObjectOfType<PlayerRespawn>();
        }

        public void SetupFloors()
        {
            foreach (var floor in Floors)
            {
                floor.GetComponents();
            }
            
            UpdateFloors();
            
            Debug.Log("Floors have been setup successfully!");
        }

        public void ResetAll()
        {
            for (int i = 0; i < Floors.Count; i++)
            {
                Floor floor = Floors[i];
                
                floor.Scale(1);
                floor.SetFloorState(Floor.FloorState.All);
                floor.SetSortingLayer("CurrentFloor",0);
            }
            
            Debug.Log("Floors have been reset successfully!");
        }
    }
}
