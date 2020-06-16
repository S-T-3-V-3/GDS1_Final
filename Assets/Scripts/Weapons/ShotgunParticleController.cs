using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ShotgunParticleController : MonoBehaviour
{
    public float damageAmount;
    public Vector3 initVelocity;
    public GameObject owningObject;

    void OnParticleCollision(GameObject other) {
        if (other == owningObject || owningObject == null) return;

        if (other.GetComponent<BasicProjectile>() != null) return;

        if (other.GetComponent<IDamageable>() != null)
        {
            DamageType damage;
            damage.owningObject = owningObject;
            damage.impactPosition = other.transform.position;
            damage.impactVelocity = this.initVelocity;
            damage.damageAmount = this.damageAmount;
            damage.isCrit = false;
            damage.isPiercing = false;

            other.GetComponent<IDamageable>().OnReceivedDamage(damage, other.transform.position, damage.impactVelocity.normalized, damage.impactVelocity.magnitude);
        }
    }
}