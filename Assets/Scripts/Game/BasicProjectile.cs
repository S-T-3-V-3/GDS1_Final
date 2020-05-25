using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BasicProjectile : MonoBehaviour
{
    public float range;
    public float damageAmount;
    public GameObject owningObject;

    Vector3 startPos;
    Rigidbody projectileRB;
    public Vector3 previousVelocity;

    private void Awake()
    {
        projectileRB = gameObject.GetComponent<Rigidbody>();
    }

    private void Start()
    {
        startPos = gameObject.transform.position;
        Destroy(gameObject, 3f);
    }

    public void SetBulletVelocity(Vector3 force)
    {
        projectileRB.velocity = force;
    }

    private void Update() {
        if (Vector3.Magnitude(this.gameObject.transform.position - startPos) > range) {
            GameObject.Destroy(this.gameObject);
        }

        previousVelocity = projectileRB.velocity;

        if (projectileRB.velocity != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(projectileRB.velocity);
    }

    private void OnCollisionEnter(Collision other) {
        if (other.gameObject == owningObject || owningObject == null) return;

        if (other.gameObject.GetComponent<IDamageable>() != null)
        {
            DamageType damage;
            damage.owningObject = owningObject;
            damage.impactPosition = other.contacts.First().point;
            damage.impactVelocity = previousVelocity;
            damage.damageAmount = this.damageAmount;
            damage.isCrit = false;
            damage.isPiercing = false;

            other.gameObject.GetComponent<IDamageable>().OnReceivedDamage(damage, damage.impactPosition, damage.impactVelocity.normalized, damage.impactVelocity.magnitude);

            GameObject.Destroy(this.gameObject);
        }
    }
}
