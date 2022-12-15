using System.Collections;
using CameraScripts;
using DG.Tweening;
using LD;
using UnityEngine;
using UnityEngine.UI;

namespace Player
{
    public class PlayerRespawn : MonoBehaviour
    {
        [SerializeField] private PlayerController playerController;
        [SerializeField] private Image respawnScreen;

        private Checkpoint currentCheckpoint;
        private PlayerData data => playerController.Data;
        private Health health => playerController.Health;
        
        [Header("Debug")]
        [SerializeField] private KeyCode respawnKey;
        [SerializeField] private KeyCode immortalityKey;
        
        private void OnEnable()
        {
            Checkpoint.onCheckpointEnter += SetCheckpoint;
        }

        private void OnDisable()
        {
            Checkpoint.onCheckpointEnter -= SetCheckpoint;
        }

        //TODO: To remove
        private void Update()
        {
            if (Input.GetKeyDown(respawnKey))
            {
                Respawn();
            }
            
            else if (Input.GetKeyDown(immortalityKey))
            {
                health.FlipImmortality();
            }
        }
        
        public void SetCheckpoint(Checkpoint newCheckpoint)
        {
            currentCheckpoint = newCheckpoint;
        }

        public void Spawn()
        {
            Transform checkpointTransform = currentCheckpoint.transform;
            transform.SetPositionAndRotation(checkpointTransform.position, checkpointTransform.rotation);
            CameraController.TeleportToTarget();
        }

        public void Respawn()
        {
            StartCoroutine(RespawnCR());
        }

        private IEnumerator RespawnCR()
        {
            respawnScreen.DOFade(1, data.respawnFadeIn).SetEase(Ease.Linear);
            yield return new WaitForSeconds(data.respawnFadeIn);

            Spawn();
            health.Heal();
            yield return new WaitForSeconds(data.respawnStay);
        
            respawnScreen.DOFade(0, data.respawnFadeOut).SetEase(Ease.Linear);
            yield return new WaitForSeconds(data.respawnFadeOut);
        }
    }
}
