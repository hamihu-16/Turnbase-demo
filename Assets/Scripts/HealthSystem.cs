using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float health;

    public event EventHandler OnDead;
    public event EventHandler OnHealthChanged;

    private void Awake()
    {
        //health = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;

        if (health <= 0)
        {
            health = 0;
            Die();
            Destroy(gameObject);
        }

        OnHealthChanged?.Invoke(this, EventArgs.Empty);
    }

    public void Heal(float healAmount)
    {
        health += healAmount;
        if (health >= maxHealth)
        {
            health = maxHealth;
        }
        OnHealthChanged?.Invoke(this, EventArgs.Empty);
    }

    private void Die()
    {
        Debug.Log(gameObject + "die");
        OnDead?.Invoke(this, EventArgs.Empty);
    }

    public float GetHealth()
    {
        return this.health;
    }

    public float getMaxHealth()
    {
        return this.maxHealth;
    }

}
