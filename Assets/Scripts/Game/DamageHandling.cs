using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    void InitDamageable();
    void OnReceivedDamage(DamageType damageType, Vector3 hitPoint, Vector3 hitDirection, float hitSpeed);
    void OnDeath(Vector3 hitPoint, Vector3 hitDirection, float hitSpeed);
}

public struct DamageType {
    public GameObject owningObject;
    public Vector3 impactPosition;
    public Vector3 impactVelocity;
    public float damageAmount;
    public bool isCrit;
    public bool isPiercing;
}