using System.Collections;
using BulletPro;
using UnityEngine;

public class SpeakerMinionShoot : MonoBehaviour
{
    [SerializeField] private BulletEmitter emitter;
    [SerializeField] private float timeBetweenLasers;

    private void Start()
    {
        StartCoroutine(Shoot());
    }

    private IEnumerator Shoot()
    {
        emitter.Play();
        yield return new WaitForSeconds(timeBetweenLasers);
        emitter.Stop();

        StartCoroutine(Shoot());
    }
}
