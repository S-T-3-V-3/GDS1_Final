using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

public class BasicEnemy : MonoBehaviour//, IDamageable
{
    /*public ObjectStats enemyStats;
    public UnityEvent OnHealthChanged;
    GameManager gameManager;
    public string enemyType = "Runner";
    

    void Start() {
        gameManager = GameManager.Instance;
        InitDamageable();
    }

    public void OnReceivedDamage(DamageType damageType)
    {
        enemyStats.currentHealth -= damageType.damageAmount;
        OnHealthChanged.Invoke();

        if (enemyStats.currentHealth <= 0)
            OnDeath();
        
        if (damageType.isCrit) {
            // Play particle effect at location
        }
    }

    public void InitDamageable()
    {
        enemyStats = gameManager.gameSettings.Enemies.Where(x => x.EnemyName == enemyType).First().enemyStats;
    }

    public void OnDeath()
    {
        // Play cool effect on enemy
        // Add to player's score
        Debug.Log("Enemy is Dead");
    }*/
}
