using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LegacyBosses.Dependencies
{
    public class BossBar : MonoBehaviour
    {
        [field:SerializeField] public Health Health { get; private set; }
        [SerializeField] private Image healthFill;
        [SerializeField] private TMP_Text healthAmountText;
        [SerializeField] private float introFillSpeed = 1f;

        private float fillCurrent;

        public void InitBar() {
            DOTween.To(() => fillCurrent, x => fillCurrent = x, Health.MaxHealth, introFillSpeed).OnUpdate(() => {
                healthAmountText.text = ((int)fillCurrent).ToString();
                healthFill.fillAmount = fillCurrent/Health.MaxHealth;
            });
        }
        
        public void UpdateBar()
        {
            healthFill.DOKill();
            healthFill.DOFillAmount(Health.GetRatio(), 0.5f);
            healthAmountText.text = Health.CurrentHealth.ToString();
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}
