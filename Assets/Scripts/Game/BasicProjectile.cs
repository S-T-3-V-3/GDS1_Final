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
    Rigidbody rb;

    private void Start()
    {
        startPos = this.gameObject.transform.position;
        rb = this.gameObject.GetComponent<Rigidbody>();
        Destroy(gameObject, 3f);
        //transform.rotation = Quaternion.LookRotation(rb.velocity);
    }

    private void Update() {
        if (Vector3.Magnitude(this.gameObject.transform.position - startPos) > range) {
            GameObject.Destroy(this.gameObject);
        }

        transform.rotation = Quaternion.LookRotation(rb.velocity);
    }

    private void OnCollisionEnter(Collision other) {
        if (other.gameObject == owningObject || owningObject == null) return;

        if (other.gameObject.GetComponent<IDamageable>() != null)
        {
            DamageType damage;
            damage.owningObject = owningObject;
            damage.impactPosition = other.contacts.First().point;
            damage.impactVelocity = this.gameObject.GetComponent<Rigidbody>().velocity;
            damage.damageAmount = this.damageAmount;
            damage.isCrit = false;
            damage.isPiercing = false;

            other.gameObject.GetComponent<IDamageable>().OnReceivedDamage(damage);

            GameObject.Destroy(this.gameObject);
        }
    }
}
