using System;
using DG.Tweening;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;

namespace Player
{
    public class PlayerEnergy : MonoBehaviour
    {
        public static float gameSpeed = 1;
    
        [SerializeField] private Image timeFill;
        [SerializeField] private PlayerController playerController;

        private PlayerData playerData => playerController.data;

        public void SpeedUpTime()
        {
            gameSpeed = playerData.speedUpModifier;

            if (Math.Abs(timeFill.fillAmount - 1) < 0.01f) return;

            timeFill.DOKill();
            timeFill.DOFillAmount(1, playerData.speedDownMaxDuration - playerData.speedDownMaxDuration * timeFill.fillAmount).SetEase(Ease.Linear)
                .OnComplete(ResetSpeed);
        }

        public void SlowDownTime()
        {
            if (timeFill.fillAmount == 0) return;

            gameSpeed = playerData.speedDownModifier;
            timeFill.DOKill();
            timeFill.DOFillAmount(0, playerData.speedDownMaxDuration * timeFill.fillAmount).SetEase(Ease.Linear).OnComplete(ResetSpeed);
        }

        public void ResetSpeed()
        {
            timeFill.DOKill();
            gameSpeed = 1;
        }
    }
}
