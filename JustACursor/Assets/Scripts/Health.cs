using System;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    public int MaxHealth => maxHealth;
    public int CurrentHealth => currentHealth;

    private int maxHealth = -1;
    private int currentHealth = -1;
    
    public UnityEvent onHealthGain;
    public UnityEvent onHealthLose;
    public UnityEvent onDeath;

    private void Start()
    {
        currentHealth = MaxHealth;
    }

    public void Init(int maxHealth)
    {
        this.maxHealth = maxHealth;
        currentHealth = maxHealth;
    }

    public void GainHealth(int amount)
    {
        if (maxHealth == -1) Debug.LogError("Health has not been initialized!");
        
        currentHealth = Math.Clamp(currentHealth + amount, 0, MaxHealth);
        onHealthGain.Invoke();
    }

    public void LoseHealth(int amount)
    {
        if (maxHealth == -1) Debug.LogError("Health has not been initialized!");
        
        currentHealth = Math.Clamp(currentHealth - amount, 0, MaxHealth);
        onHealthLose.Invoke();
        
        if (currentHealth == 0) onDeath.Invoke();
    }

    public float GetRatio()
    {
        return (float)currentHealth / maxHealth;
    }

    public void SetHealth(int amount)
    {
        if (amount > maxHealth) Debug.LogError("Can't set health above max health!");
        currentHealth = amount;
    }

    public void Heal() {
        GainHealth(MaxHealth);
    }

    public void Kill() {
        LoseHealth(MaxHealth);
    }
}
