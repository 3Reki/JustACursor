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
        [SerializeField] private Image timeFill;

        private PlayerData data => playerController.data;

        public void SpeedUpTime()
        {
            if (!energy.SpeedUpTime()) return;
            
            timeFill.DOKill();
            timeFill.DOFillAmount(1, data.timeAbilityDuration - data.timeAbilityDuration * timeFill.fillAmount).SetEase(Ease.Linear);
            
            onPlayerSpeedUp.Invoke();
        }

        public void SlowDownTime()
        {
            if (!energy.SlowDownTime()) return;
            if (timeFill.fillAmount == 0) return;
            
            timeFill.DOKill();
            timeFill.DOFillAmount(0, data.timeAbilityDuration * timeFill.fillAmount).SetEase(Ease.Linear);
            
            onPlayerSlowDown.Invoke();
        }

        public void ResetSpeed()
        {
            if (!energy.ResetSpeed()) return;

            timeFill.DOKill();
            
            onPlayerReset.Invoke();
        }

        public UnityEvent onPlayerSpeedUp;
        public UnityEvent onPlayerSlowDown;
        public UnityEvent onPlayerReset;
        
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
