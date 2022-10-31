using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;

namespace Player {
    public class PlayerUI : MonoBehaviour
    {
        [SerializeField] private PlayerController playerController;
        [SerializeField] private Health health;
        [SerializeField] private List<Image> pvs;
        [SerializeField] private Image energy;
        
        private PlayerData data => playerController.data;
        private Coroutine showHealthCoroutine;
    
        public void ShowHealth()
        {
            if (showHealthCoroutine != null)
            {
                Debug.Log("Kill");
                foreach (Image image in pvs)
                {
                    image.DOKill();
                }
                StopCoroutine(showHealthCoroutine);
            }
            showHealthCoroutine = StartCoroutine(ShowHealthCR());
        }
    
        private void HideHealth()
        {
            foreach (Image image in pvs)
            {
                image.DOFade(0, data.healthFadeOut);
            }
        }
        
        private IEnumerator ShowHealthCR()
        {
            for (int i = 0; i < pvs.Count; i++)
            {
                if (health.GetCurrentHealth() >= health.GetMaxHealth() - i) {
                    pvs[i].DOFade(1, data.healthFadeIn);
                }
                else pvs[i].DOFade(0, data.healthFadeOut);
            }
           
            yield return new WaitForSeconds(data.healthFadeIn+data.healthStay);
            HideHealth();
        }
        
        
        public void ShowEnergy()
        {
            energy.DOFade(1, data.timeFadeIn);
        }
    
        public void HideEnergy()
        {
            energy.DOFade(0, data.timeFadeOut);
        }
    }
}

