using System;
using DG.Tweening;
using ScriptableObjects;
using UnityEngine;
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
        
        [SerializeField] private Image timeFill;
        [SerializeField] private PlayerController playerController;

        private static float _gameSpeed = 1;
        private PlayerData playerData => playerController.data;
        private float tweenTime;
        private TimeState timeState = TimeState.Resetting;

        public void SpeedUpTime()
        {
            if (timeState == TimeState.Speeding) return;
            
            timeState = TimeState.Speeding;
            
            DOTween.Kill(GameSpeed);
            tweenTime = playerData.lerpDuration - Mathf.Lerp(0, playerData.lerpDuration, (GameSpeed - 1) / (playerData.speedUpModifier - 1));
            DOTween.To(() => GameSpeed, x => GameSpeed = x, playerData.speedUpModifier, tweenTime).SetEase(playerData.lerpEase);

            timeFill.DOKill();
            timeFill.DOFillAmount(1, playerData.timeAbilityDuration - playerData.timeAbilityDuration * timeFill.fillAmount).SetEase(Ease.Linear);
        }

        public void SlowDownTime()
        {
            if (timeState == TimeState.Slowing) return;
            if (timeFill.fillAmount == 0) return;
            
            timeState = TimeState.Slowing;

            DOTween.Kill(GameSpeed);
            tweenTime = playerData.lerpDuration - Mathf.Lerp(0, playerData.lerpDuration, (1-GameSpeed)/(1-playerData.speedDownModifier));
            DOTween.To(() => GameSpeed, x => GameSpeed = x, playerData.speedDownModifier, tweenTime).SetEase(playerData.lerpEase);
            
            timeFill.DOKill();
            timeFill.DOFillAmount(0, playerData.timeAbilityDuration * timeFill.fillAmount).SetEase(Ease.Linear);
        }

        public void ResetSpeed()
        {
            if (timeState == TimeState.Resetting) return;
            
            timeState = TimeState.Resetting;

            timeFill.DOKill();

            DOTween.Kill(GameSpeed);
            if (GameSpeed > 1) tweenTime = Mathf.Lerp(0,playerData.lerpDuration, (GameSpeed - 1) / (playerData.speedUpModifier - 1));
            else tweenTime = Mathf.Lerp(0,playerData.lerpDuration,(1-GameSpeed)/(1-playerData.speedDownModifier));
            DOTween.To(() => GameSpeed, x => GameSpeed = x, 1, playerData.lerpDuration).SetEase(Ease.Linear);
        }
        
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
