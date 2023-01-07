using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Player
{
    public class PlayerEnergy : MonoBehaviour
    {
        [SerializeField] private PlayerController playerController;
        [SerializeField] private Energy energy;
        [SerializeField] private Image extraTimeFill;
        [SerializeField] private Image intraTimeFill;

        [Header("Feedback")]
        [SerializeField] private Animator animator;

        private PlayerData data => playerController.Data;

        public void SpeedUpTime()
        {
            if (energy.timeState == Energy.TimeState.Speeding) return;
            energy.SpeedUpTime();

            if (extraTimeFill.fillAmount < 1)
            {
                float duration = data.timeAbilityDuration - data.timeAbilityDuration * intraTimeFill.fillAmount;
                DoFillAmount(intraTimeFill,1,duration);
                DoFillAmount(extraTimeFill,1,duration, () =>
                {
                    animator.Play("Energy@Scale");
                });
            }
            
            onPlayerSpeedUp.Invoke();
        }

        public void SlowDownTime()
        {
            if (extraTimeFill.fillAmount == 0)
            {
                ResetSpeed();
                return;
            }
            if (energy.timeState == Energy.TimeState.Slowing) return;
            
            energy.SlowDownTime();
            
            float duration = data.timeAbilityDuration * intraTimeFill.fillAmount;
            DoFillAmount(intraTimeFill,0,duration);
            DoFillAmount(extraTimeFill,0,duration);
            
            onPlayerSlowDown.Invoke();
        }

        public void ResetSpeed()
        {
            if (energy.timeState == Energy.TimeState.Resetting) return;
            energy.ResetSpeed();
            
            intraTimeFill.DOKill();
            extraTimeFill.DOKill();
            
            onPlayerReset.Invoke();
        }

        private void DoFillAmount(Image image, float endValue, float duration, TweenCallback OnComplete = null)
        {
            image.DOKill();
            image.DOFillAmount(endValue, duration).SetEase(Ease.Linear).OnComplete(OnComplete);
        }

        public UnityEvent onPlayerSpeedUp;
        public UnityEvent onPlayerSlowDown;
        public UnityEvent onPlayerReset;
    }
}
