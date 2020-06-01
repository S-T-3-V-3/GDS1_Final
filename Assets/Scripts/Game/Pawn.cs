using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pawn : MonoBehaviour, IDamageable
{
    public StatHandler statHandler;
    public Weapon equippedWeapon;
    public Transform firePoint;
    public GameObject deathEffectPrefab;

    bool canTakeDamage = true;

    public abstract void InitDamageable();

    public abstract void OnDeath(Vector3 hitPoint, Vector3 hitDirection, float hitSpeed);

    public virtual void OnReceivedDamage(DamageType damageType, Vector3 hitPoint, Vector3 hitDirection, float hitSpeed)
    {
        if (canTakeDamage == false) return;

        statHandler.CurrentHealth -= damageType.damageAmount;

        if (statHandler.CurrentHealth <= 0)
            OnDeath(hitPoint, hitDirection, hitSpeed);
        
        if (damageType.isCrit) {
            // Do something
        }
    }
}
