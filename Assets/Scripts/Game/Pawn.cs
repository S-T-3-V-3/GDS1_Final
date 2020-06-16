using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public abstract class Pawn : MonoBehaviour, IDamageable, IPausable
{
    public StatHandler statHandler;
    public Weapon equippedWeapon;
    public Transform firePoint;

    public bool isPaused = false;

    public bool canTakeDamage = true;

    public abstract void InitDamageable();

    public abstract void OnDeath(Vector3 hitPoint, Vector3 hitDirection, float hitSpeed);

    public virtual void OnReceivedDamage(DamageType damageType, Vector3 hitPoint, Vector3 hitDirection, float hitSpeed)
    {
        if (canTakeDamage == false) return;

        if (damageType.isCrit)
            statHandler.CurrentHealth -= damageType.damageAmount * 2;
        else
            statHandler.CurrentHealth -= damageType.damageAmount;

        if (statHandler.CurrentHealth <= 0)
            OnDeath(hitPoint, hitDirection, hitSpeed);
    }

    public virtual void Pause() {
        isPaused = true;

        if (this.GetComponent<Animator>() != null) {
            this.GetComponent<Animator>().StopPlayback();
        }
    }
    public virtual void UnPause() {
        isPaused = false;

        if (this.GetComponent<Animator>() != null) {
            this.GetComponent<Animator>().StartPlayback();
        }
    }
}
