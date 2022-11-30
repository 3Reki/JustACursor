using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Player
{
    public class PlayerRespawn : MonoBehaviour
    {
        [SerializeField] private PlayerController playerController;
        [SerializeField] private Image respawnScreen;
        [SerializeField] private Transform respawnPosition;

        [Header("Debug")]
        [SerializeField] private KeyCode respawnKey;

        private PlayerData data => playerController.data;

        public bool isAlive { get; private set; } = true;

        //TODO: To remove
        private void Update()
        {
            if (Input.GetKeyDown(respawnKey))
            {
                Respawn();
            }
        }

        public void Respawn()
        {
            StartCoroutine(RespawnCR());
        }

        public void SetCheckpoint(Transform newCheckpoint)
        {
            respawnPosition.position = newCheckpoint.position;
        }

        private IEnumerator RespawnCR()
        {
            isAlive = false;
            respawnScreen.DOFade(1, data.respawnFadeIn).SetEase(Ease.Linear);
            yield return new WaitForSeconds(data.respawnFadeIn);
        
            transform.position = respawnPosition.position;
            yield return new WaitForSeconds(data.respawnStay);
        
            respawnScreen.DOFade(0, data.respawnFadeOut).SetEase(Ease.Linear);
            yield return new WaitForSeconds(data.respawnFadeOut);
            isAlive = true;
        }
    }
}
