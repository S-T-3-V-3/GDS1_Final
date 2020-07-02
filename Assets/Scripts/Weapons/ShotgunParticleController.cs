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
            //Responsible to preventing enemy fire
            if(owningObject.GetComponent<BasicEnemy>() != null && other.gameObject.GetComponent<BasicEnemy>()) return;

            DamageType damage;
            damage.owningObject = owningObject;
            damage.impactPosition = other.transform.position;
            damage.impactVelocity = this.initVelocity;
            damage.damageAmount = this.damageAmount;
            damage.isCrit = false;
            damage.isPiercing = false;

            if(owningObject.GetComponent<BasicPlayer>() !=null)
            {
                BasicPlayer player = owningObject.GetComponent<BasicPlayer>();
                if(Random.Range(0, 101) <= player.statHandler.CritChance)
                    damage.isCrit = true;
            }

            other.GetComponent<IDamageable>().OnReceivedDamage(damage, other.transform.position, damage.impactVelocity.normalized, damage.impactVelocity.magnitude);
        }
    }
}