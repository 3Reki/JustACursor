using System;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [SerializeField] private int maxHealth;
    private int currentHealth;

    private ScriptableObject healthData;
    public UnityEvent onHealthGain;
    public UnityEvent onHealthLose;
    public UnityEvent onDeath;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void GainHealth(int amount)
    {
        currentHealth = Math.Clamp(currentHealth + amount, 0, maxHealth);
        onHealthGain.Invoke();
    }

    public void LoseHealth(int amount)
    {
        currentHealth = Math.Clamp(currentHealth - amount, 0, maxHealth);
        onHealthLose.Invoke();
        
        if (currentHealth == 0) onDeath.Invoke();
    }

    public void Heal() {
        GainHealth(maxHealth);
    }

    public void Kill() {
        LoseHealth(maxHealth);
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    public int GetMaxHealth()
    {
        return maxHealth;
    }
}