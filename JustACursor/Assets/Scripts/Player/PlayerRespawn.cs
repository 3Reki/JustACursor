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

        private Checkpoint respawnPoint;
        
        [Header("Debug")]
        [SerializeField] private KeyCode respawnKey;

        private PlayerData data => playerController.data;
        public bool isAlive { get; private set; } = true;
        
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
        }

        public void Spawn()
        {
            Transform checkpointTransform = respawnPoint.transform;
            transform.SetPositionAndRotation(checkpointTransform.position, checkpointTransform.rotation);
            CameraController.TeleportToTarget();
        }

        public void SetCheckpoint(Checkpoint newCheckpoint)
        {
            respawnPoint = newCheckpoint;
        }
        
        private void Respawn()
        {
            StartCoroutine(RespawnCR());
        }

        private IEnumerator RespawnCR()
        {
            isAlive = false;
            respawnScreen.DOFade(1, data.respawnFadeIn).SetEase(Ease.Linear);
            yield return new WaitForSeconds(data.respawnFadeIn);

            Spawn();
            yield return new WaitForSeconds(data.respawnStay);
        
            respawnScreen.DOFade(0, data.respawnFadeOut).SetEase(Ease.Linear);
            yield return new WaitForSeconds(data.respawnFadeOut);
            isAlive = true;
        }
    }
}
