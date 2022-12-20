using System;
using Player;
using UnityEngine;

namespace Bosses.Dependencies
{
    public class BossTrigger : MonoBehaviour
    {
        [SerializeField] private Boss bossToSpawn;
        [SerializeField] private GameObject invisibleWalls;

        private bool isActive;
        private PlayerController controller;

        private void OnTriggerEnter2D(Collider2D other)
        {
            controller = other.GetComponent<PlayerController>();
            if (!controller)
            {
                return;
            }

            controller.Health.onDeath.AddListener(Undo);
            isActive = true;
            Do();
        }

        private void OnDestroy()
        {
            if (isActive)
            {
                controller.Health.onDeath.RemoveListener(Undo);
            }
        }

        public void Do()
        {
            bossToSpawn.gameObject.SetActive(true);
            invisibleWalls.SetActive(true);
            enabled = false;
        }

        public void Undo()
        {
            bossToSpawn.Reset();
            bossToSpawn.gameObject.SetActive(false);
            invisibleWalls.SetActive(false);
            enabled = true;
            isActive = false;
            controller.Health.onDeath.RemoveListener(Undo);
        }
        
#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Bounds triggerBounds = GetComponent<BoxCollider2D>().bounds;
            Vector3 position = triggerBounds.center;
            Vector3 size = triggerBounds.size;
            
            Gizmos.color = new Color(1,1, 1, .5f);
            Gizmos.DrawCube(position, size);
            
            Gizmos.color = new Color(0, 0, 0);
            Gizmos.DrawWireCube(position, size);
        }
#endif
    }
}