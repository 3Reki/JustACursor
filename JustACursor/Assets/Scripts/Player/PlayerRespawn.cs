using System.Collections;
using DG.Tweening;
using Player;
using UnityEngine;
using UnityEngine.UI;

public class PlayerRespawn : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private Transform checkpoint;

    [Header("Screen")]
    [SerializeField] private Image blackScreen;
    [SerializeField, Range(0.1f, 1)] private float fadeInDuration;
    [SerializeField, Range(0.1f, 1)] private float stayDuration;
    [SerializeField, Range(0.1f, 1)] private float fadeOutDuration;

    public bool isAlive { get; private set; } = true;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Respawn();
        }
    }

    public void Respawn()
    {
        StartCoroutine(RespawnCR());
    }

    private IEnumerator RespawnCR()
    {
        isAlive = false;
        blackScreen.DOFade(1, fadeInDuration).SetEase(Ease.Linear);
        yield return new WaitForSeconds(fadeInDuration);
        
        transform.position = checkpoint.position;
        yield return new WaitForSeconds(stayDuration);
        
        blackScreen.DOFade(0, fadeOutDuration).SetEase(Ease.Linear);
        yield return new WaitForSeconds(fadeOutDuration);
        isAlive = true;
    }
}
