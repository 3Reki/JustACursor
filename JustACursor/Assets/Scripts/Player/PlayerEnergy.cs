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

        public void SpeedUpTime()
        {
            GameSpeed = playerData.speedUpModifier;

            if (Math.Abs(timeFill.fillAmount - 1) < 0.01f) return;

            timeFill.DOKill();
            timeFill.DOFillAmount(1, playerData.timeAbilityDuration - playerData.timeAbilityDuration * timeFill.fillAmount)
                .SetEase(Ease.Linear).OnComplete(ResetSpeed);
        }

        public void SlowDownTime()
        {
            if (timeFill.fillAmount == 0) return;

            GameSpeed = playerData.speedDownModifier;
            
            timeFill.DOKill();
            timeFill.DOFillAmount(0, playerData.timeAbilityDuration * timeFill.fillAmount)
                .SetEase(Ease.Linear).OnComplete(ResetSpeed);
        }

        public void ResetSpeed()
        {
            timeFill.DOKill();
            GameSpeed = 1;
        }
        
        public delegate void OnGameSpeedUpdate();
        public static OnGameSpeedUpdate onGameSpeedUpdate;
    }
}
