using DG.Tweening;
using ScriptableObjects;
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

        private PlayerData data => playerController.data;

        public void SpeedUpTime()
        {
            if (!energy.SpeedUpTime()) return;
            
            DOFillAmount(intraTimeFill,1,data.timeAbilityDuration - data.timeAbilityDuration * intraTimeFill.fillAmount);
            DOFillAmount(extraTimeFill,1,data.timeAbilityDuration - data.timeAbilityDuration * extraTimeFill.fillAmount);
            
            onPlayerSpeedUp.Invoke();
        }

        public void SlowDownTime()
        {
            if (!energy.SlowDownTime()) return;
            if (intraTimeFill.fillAmount == 0) return;
            
            DOFillAmount(intraTimeFill,0,data.timeAbilityDuration * intraTimeFill.fillAmount);
            DOFillAmount(extraTimeFill,0,data.timeAbilityDuration * extraTimeFill.fillAmount);
            
            onPlayerSlowDown.Invoke();
        }

        public void ResetSpeed()
        {
            if (!energy.ResetSpeed()) return;

            intraTimeFill.DOKill();
            extraTimeFill.DOKill();
            
            onPlayerReset.Invoke();
        }

        private void DOFillAmount(Image image, float endValue, float duration)
        {
            image.DOKill();
            image.DOFillAmount(endValue, duration).SetEase(Ease.Linear);
        }

        public UnityEvent onPlayerSpeedUp;
        public UnityEvent onPlayerSlowDown;
        public UnityEvent onPlayerReset;
    }
}
