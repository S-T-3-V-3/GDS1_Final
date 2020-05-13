using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

public class BasicEnemy : MonoBehaviour, IDamageable
{
    public UnityEvent OnHealthChanged;
    GameManager gameManager;  
    ObjectStats objectStats;  
    EnemyStats enemyStats;

    public void Init(EnemyType enemyType) {
        gameManager = GameManager.Instance;
        this.objectStats = enemyType.objectStats;
        this.enemyStats = enemyType.enemyStats;

        InitDamageable();
    }

    public void OnReceivedDamage(DamageType damageType)
    {
        objectStats.currentHealth -= damageType.damageAmount;
        OnHealthChanged.Invoke();

        if (objectStats.currentHealth <= 0)
            OnDeath();
        
        if (damageType.isCrit) {
            // Play particle effect at location
        }
    }

    public void InitDamageable()
    {
        objectStats.currentHealth = objectStats.maxHealth;
    }

    public void OnDeath()
    {
        // Play cool effect on enemy
        // Add to player's score
        GameObject.Destroy(this.gameObject);
        Debug.Log("Enemy is Dead");
    }
}
