using DG.Tweening;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Player
{
    public class PlayerEnergy : MonoBehaviour
    {
        public static float GameSpeed
        {
            get => _gameSpeed;
            private set
            {
                _gameSpeed = value;
                onGameSpeedUpdate?.Invoke();
            }
        }

        [SerializeField] private PlayerController playerController;
        [SerializeField] private Image timeFill;

        private static float _gameSpeed = 1;
        private PlayerData data => playerController.data;
        private float tweenTime;
        private TimeState timeState = TimeState.Resetting;

        public void SpeedUpTime(float value)
        {
            if (timeState == TimeState.Speeding) return;
            
            timeState = TimeState.Speeding;
            
            DOTween.Kill(GameSpeed);
            tweenTime = data.lerpDuration - Mathf.Lerp(0, data.lerpDuration, (GameSpeed - 1) / (value - 1));
            DOTween.To(() => GameSpeed, x => GameSpeed = x, value, tweenTime).SetEase(data.lerpEase);

            timeFill.DOKill();
            timeFill.DOFillAmount(1, data.timeAbilityDuration - data.timeAbilityDuration * timeFill.fillAmount).SetEase(Ease.Linear);
            
            onSpeedUp.Invoke();
        }

        public void SlowDownTime(float value)
        {
            if (timeState == TimeState.Slowing) return;
            if (timeFill.fillAmount == 0) return;
            
            timeState = TimeState.Slowing;

            DOTween.Kill(GameSpeed);
            tweenTime = data.lerpDuration - Mathf.Lerp(0, data.lerpDuration, (1-GameSpeed)/(1-value));
            DOTween.To(() => GameSpeed, x => GameSpeed = x, value, tweenTime).SetEase(data.lerpEase);
            
            timeFill.DOKill();
            timeFill.DOFillAmount(0, data.timeAbilityDuration * timeFill.fillAmount).SetEase(Ease.Linear);
            
            onSlowDown.Invoke();
        }

        public void ResetSpeed()
        {
            if (timeState == TimeState.Resetting) return;
            
            timeState = TimeState.Resetting;

            timeFill.DOKill();

            DOTween.Kill(GameSpeed);
            if (GameSpeed > 1) tweenTime = Mathf.Lerp(0,data.lerpDuration, (GameSpeed - 1) / (data.speedUpModifier - 1));
            else tweenTime = Mathf.Lerp(0,data.lerpDuration,(1-GameSpeed)/(1-data.slowDownModifier));
            DOTween.To(() => GameSpeed, x => GameSpeed = x, 1, data.lerpDuration).SetEase(Ease.Linear);
            
            onReset.Invoke();
        }

        public UnityEvent onSpeedUp;
        public UnityEvent onSlowDown;
        public UnityEvent onReset;
        
        public delegate void OnGameSpeedUpdate();
        public static OnGameSpeedUpdate onGameSpeedUpdate;
        
        private enum TimeState
        {
            Slowing,
            Resetting,
            Speeding
        }
    }
}
