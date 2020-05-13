using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BasicPlayer : MonoBehaviour, IDamageable
{
    public ObjectStats playerStats;
    public UnityEvent OnHealthChanged;
    GameManager gameManager;
    

    void Start() {
        gameManager = GameManager.Instance;
        InitDamageable();
    }

    public void OnReceivedDamage(DamageType damageType)
    {
        if (playerStats.canTakeDamage == false) return;

        playerStats.currentHealth -= damageType.damageAmount;
        OnHealthChanged.Invoke();

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
        playerStats.currentStamina = playerStats.maxStamina;
    }

    public void OnDeath()
    {
        // Play cool effect on player
        float deathAnimationSeconds = 1;
        
        gameManager.Invoke("GameOver", deathAnimationSeconds);
    }
}