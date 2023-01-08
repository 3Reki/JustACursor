using System.Collections;
using Player;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace CameraScripts
{
    public class CameraEffect : MonoBehaviour
    {
        [SerializeField] private Camera mainCamera;
        [SerializeField] private Volume volume;

        private Coroutine speedEffectCoroutine;
        private LensDistortion lensDistortion;
        private ColorAdjustments colorAdjustments;

        [Header("Base")]
        [SerializeField, Tooltip("Time to reset"), Range(0,1)] private float resetTime;
        private float baseOrthoSize;
        private float baseDistortion;
        
        [Header("Speed Up")]
        [SerializeField, Tooltip("Time to reach speed up effect"), Range(0,1)] private float speedTime;
        [SerializeField, Range(1,18)] private float speedOrthoSize;
        [SerializeField, Range(-1,1)] private float speedDistortion;
        [SerializeField] private Color speedColorFilter;
        
        [Header("Slow Down")]
        [SerializeField, Tooltip("Time to reach slow down effect"), Range(0,1)] private float slowTime;
        [SerializeField, Range(1,18)] private float slowOrthoSize;
        [SerializeField, Range(-1,1)] private float slowDistortion;
        [SerializeField] private Color slowColorFilter;

        private void OnEnable()
        {
            PlayerEnergy.onPlayerReset += ResetEffect;
            PlayerEnergy.onPlayerSpeedUp += SpeedUpEffect;
            PlayerEnergy.onPlayerSlowDown += SlowDownEffect;
        }

        private void OnDisable()
        {
            PlayerEnergy.onPlayerReset -= ResetEffect;
            PlayerEnergy.onPlayerSpeedUp -= SpeedUpEffect;
            PlayerEnergy.onPlayerSlowDown -= SlowDownEffect;
        }
        
        private void Awake()
        {
            if (volume.profile.TryGet(out LensDistortion lens)) lensDistortion = lens;
            else Debug.LogError("Volume don't have LensDistortion component");
            
            if (volume.profile.TryGet(out ColorAdjustments color)) colorAdjustments = color;
            else Debug.LogError("Volume don't have ColorAdjustments component");

            baseOrthoSize = mainCamera.orthographicSize;
            baseDistortion = lens.intensity.value;
        }
        
        private void ResetEffect()
        {
            if (speedEffectCoroutine != null) StopCoroutine(speedEffectCoroutine);
            speedEffectCoroutine = StartCoroutine(SpeedEffectCR(resetTime, baseOrthoSize, baseDistortion, Color.white));
        }
        
        private void SpeedUpEffect()
        {
            if (speedEffectCoroutine != null) StopCoroutine(speedEffectCoroutine);
            speedEffectCoroutine = StartCoroutine(SpeedEffectCR(speedTime, speedOrthoSize, speedDistortion, speedColorFilter));
        }

        private void SlowDownEffect()
        {
            if (speedEffectCoroutine != null) StopCoroutine(speedEffectCoroutine);
            speedEffectCoroutine = StartCoroutine(SpeedEffectCR(slowTime, slowOrthoSize, slowDistortion, slowColorFilter));
        }
        
        private IEnumerator SpeedEffectCR(float time, float newOrthoSize, float newDistortion, Color newFilter)
        {
            
            
            float timeElapsed = 0;
            while (timeElapsed < time)
            {
                mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, newOrthoSize, timeElapsed / time);
                lensDistortion.intensity.value = Mathf.Lerp(lensDistortion.intensity.value, newDistortion, timeElapsed / time);
                colorAdjustments.colorFilter.Interp(colorAdjustments.colorFilter.value, newFilter, timeElapsed / time);
                yield return null;
                timeElapsed += Time.deltaTime;
            }
        }
    }
}
