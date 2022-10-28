using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Player;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;

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
            bool active = health.GetCurrentHealth() >= health.GetMaxHealth()-i;
            pvs[i].gameObject.SetActive(active);
            if (active) pvs[i].DOFade(1, data.healthFadeIn);
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
