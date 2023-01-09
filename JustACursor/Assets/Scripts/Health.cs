using System;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    public int MaxHealth => maxHealth;
    public int CurrentHealth => currentHealth;

    private int maxHealth = -1;
    private int currentHealth = -1;
    private bool isImmortal;
    
    public UnityEvent onHealthGain;
    //public UnityEvent onHealthReset;
    public UnityEvent onHealthLose;
    public UnityEvent onDeath;
    
    public void Init(int maxHealth)
    {
        this.maxHealth = maxHealth;
        currentHealth = maxHealth;
    }

    public void GainHealth(int amount)
    {
        if (maxHealth == -1) Debug.LogError("Health has not been initialized!");
        
        currentHealth = Math.Clamp(currentHealth + amount, 0, maxHealth);
        onHealthGain.Invoke();
    }

    public void LoseHealth(int amount)
    {
        if (maxHealth == -1) Debug.LogError("Health has not been initialized!");
        
        currentHealth = Math.Clamp(currentHealth - amount, isImmortal ? 1 : 0, maxHealth);
        onHealthLose.Invoke();

        if (currentHealth == 0) onDeath.Invoke();
    }

    public void ResetHealth() {
        SetHealth(maxHealth);
        //onHealthReset.Invoke();
    }

    public void SetHealth(int amount)
    {
        amount = Math.Clamp(amount, 0, maxHealth);
        currentHealth = amount;
    }

    public void Kill() {
        onDeath.Invoke();
    }

    public void FlipImmortality()
    {
        isImmortal = !isImmortal;
    }
    
    //Return health value between 0 and 1
    public float GetRatio()
    {
        return (float)currentHealth / maxHealth;
    }
}
