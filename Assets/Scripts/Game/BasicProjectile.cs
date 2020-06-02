using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BasicProjectile : MonoBehaviour
{
    public float range;
    public float damageAmount;
    public Vector3 initVelocity;
    public Renderer ProjectileRenderer;
    public GameObject owningObject;
    public GameObject DestroyEffecyPrefab;

    Vector3 startPos;
    Rigidbody projectileRB;

    private void Start()
    {
        projectileRB = gameObject.GetComponent<Rigidbody>();
        projectileRB.velocity = this.initVelocity;
        startPos = gameObject.transform.position;
    }

    private void FixedUpdate() {
        if (Vector3.Magnitude(this.gameObject.transform.position - startPos) > range) {
            Die();
        }

        if (projectileRB.velocity != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(projectileRB.velocity);
    }

    private void OnCollisionEnter(Collision other) {
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
            AudioManager.Instance.PlaySoundEffect(SoundType.Impact);

            Die();
        }
        else {
            Die();
        }
    }

    public void Die() {
        GameObject deathEffectObject = Instantiate(DestroyEffecyPrefab, this.transform.position, Quaternion.FromToRotation(Vector3.forward, this.gameObject.transform.forward));
        ParticleSystem.MainModule deathParticleSystem = deathEffectObject.GetComponent<ParticleSystem>().main;
        float particleLifetime = deathParticleSystem.startLifetime.constant;
        deathParticleSystem.startSpeed = this.initVelocity.magnitude;
        deathEffectObject.GetComponent<Renderer>().material = ProjectileRenderer.material;
        deathParticleSystem.startSize = 0.1f;
        deathParticleSystem.startLifetimeMultiplier = 0.5f;
        ParticleSystem.ShapeModule shape = deathEffectObject.GetComponent<ParticleSystem>().shape;
        shape.radius = 0.2f;
        Destroy(deathEffectObject, particleLifetime);
        GameObject.Destroy(this.gameObject);
    }
}
