using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicPlayer : MonoBehaviour, IDamageable
{
    public ObjectStats playerStats;

    GameManager gameManager;
    

    void Start() {
        gameManager = GameManager.Instance;
        InitDamageable();
    }

    public void OnReceivedDamage(DamageType damageType)
    {
        if (playerStats.canTakeDamage == false) return;

        playerStats.currentHealth -= damageType.damageAmount;

        if (playerStats.currentHealth <= 0)
            OnDeath();
        
        if (damageType.isCrit) {
            // Play particle effect at location
        }
    }

    public void InitDamageable()
    {
        playerStats = gameManager.gameSettings.playerSettings.baseStats;
        playerStats.currentHealth = playerStats.maxHealth;
        playerStats.currentMana = playerStats.maxMana;
    }

    public void OnDeath()
    {
        // GameManager.onPlayerDeath.Invoke();
        // Play cool effect
    }
}