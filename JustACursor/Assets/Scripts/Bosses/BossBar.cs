using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Bosses
{
    public class BossBar : MonoBehaviour
    {
        [SerializeField] private Health health;
        [SerializeField] private Image healthFill;
        [SerializeField] private TMP_Text healthAmountText;

        private void OnEnable()
        {
            health.onHealthLose.AddListener(UpdateBar);
            health.onDeath.AddListener(Hide);
        }

        private void OnDisable()
        {
            health.onHealthLose.RemoveListener(UpdateBar);
            health.onDeath.RemoveListener(Hide);
        }

        private void Start()
        {
            healthAmountText.text = health.MaxHealth.ToString();
        }
        
        private void UpdateBar()
        {
            healthFill.DOKill();
            healthFill.DOFillAmount(health.GetRatio(), 0.5f);
            healthAmountText.text = health.CurrentHealth.ToString();
        }

        private void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}