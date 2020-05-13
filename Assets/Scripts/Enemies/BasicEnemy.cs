﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

public class BasicEnemy : MonoBehaviour, IDamageable
{
    public EnemyType enemyType;
    public Transform firePoint;
    public Light spotLight;
    [Space]

    [HideInInspector] public UnityEvent OnHealthChanged;
    [HideInInspector] public EnemySettings enemySettings;
    [HideInInspector] public ObjectStats enemyStats;

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

    // Static Helper Functions
    public static float GetTargetDistance(Transform t1, Transform t2) {
        return Vector3.Magnitude(t1.position - t2.position);
    }

    public static float GetTargetDistance(Vector3 v1, Vector3 v2) {
        return Vector3.Magnitude(v1 - v2);
    }

    public static bool IsPlayerInRange(BasicEnemy enemy) {
        if (GameManager.Instance.playerController == null) return false;
        
        return Vector3.Magnitude(GameManager.Instance.playerController.transform.position - enemy.transform.position) <= enemy.enemySettings.traits.detectionRange;
    }
}
