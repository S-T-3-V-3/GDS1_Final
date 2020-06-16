using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ShotgunParticleController : MonoBehaviour
{
    public float damageAmount;
    public Vector3 initVelocity;
    public GameObject owningObject;

    void OnPArticleCollisionEnter(Collision other) {
        if (other.gameObject == owningObject || owningObject == null) return;

        if (other.gameObject.GetComponent<BasicProjectile>() != null) return;

        if (other.gameObject.GetComponent<IDamageable>() != null)
        {
            DamageType damage;
            damage.owningObject = owningObject;
            damage.impactPosition = other.contacts.First().point;
            damage.impactVelocity = this.initVelocity;
            damage.damageAmount = this.damageAmount;
            damage.isCrit = false;
            damage.isPiercing = false;

            other.gameObject.GetComponent<IDamageable>().OnReceivedDamage(damage, damage.impactPosition, damage.impactVelocity.normalized, damage.impactVelocity.magnitude);
        }
    }
}