using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    void InitDamageable();
    void OnReceivedDamage(DamageType damageType);
    void OnDeath();
}

public struct DamageType {
    public GameObject owningObject;
    public Vector3 impactPosition;
    public Vector3 impactVelocity;
    public float damageAmount;
    public bool isCrit;
    public bool isPiercing;
}