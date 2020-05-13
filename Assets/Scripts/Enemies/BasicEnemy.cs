using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

public class BasicEnemy : MonoBehaviour, IDamageable
{
    public EnemyType enemyType;
    [Space]

    public UnityEvent OnHealthChanged;

    [HideInInspector] public EnemySettings enemySettings;
    public ObjectStats enemyStats;

    EnemyStateManager stateManager;
    GameManager gameManager; 

    void Start()
    {
        gameManager = GameManager.Instance;

        enemySettings = gameManager.gameSettings.Enemies.Where(x => x.enemyType == this.enemyType).First();
        enemyStats = enemySettings.stats;

        stateManager = this.gameObject.AddComponent<EnemyStateManager>();
        SetState<EnemySpawnState>();

        InitDamageable();
    }

    public void SetState<T>() where T : EnemyState
    {
        stateManager.AddState<T>();
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
        enemyStats.currentHealth = enemyStats.maxHealth;
    }

    public void OnDeath()
    {
        // Play cool effect on enemy
        // Add to player's score
        GameObject.Destroy(this.gameObject);
        Debug.Log("Enemy is Dead");
    }
}
