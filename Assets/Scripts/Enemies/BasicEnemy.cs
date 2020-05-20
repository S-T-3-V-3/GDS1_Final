using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

public class BasicEnemy : MonoBehaviour, IDamageable
{
    public EnemyType enemyType;
    public Transform firePoint;
    public Light spotLight;
    public BasicWeapon equippedWeapon;
    Material impactMaterial;

    [Space]

    [HideInInspector] public UnityEvent OnHealthChanged;
    [HideInInspector] public EnemySettings enemySettings;
    [HideInInspector] public ObjectStats enemyStats;

    EnemyStateManager stateManager;
    GameManager gameManager;

    public GameObject deathEffectPrefab;

    void Start()
    {
        gameManager = GameManager.Instance;

        enemySettings = gameManager.gameSettings.Enemies.Where(x => x.enemyType == this.enemyType).First();
        enemyStats = enemySettings.stats;

        this.gameObject.GetComponent<Renderer>().material = enemySettings.traits.material;

        // Get Impact Material
        impactMaterial = GetComponent<MeshRenderer>().materials[1];

        if (enemySettings.weaponType != WeaponType.MELEE)
            EquipWeapon();

        stateManager = this.gameObject.AddComponent<EnemyStateManager>();
        SetState<EnemySpawnState>();

        InitDamageable();
    }

    public void SetState<T>() where T : EnemyState
    {
        stateManager.AddState<T>();
    }

    public void OnReceivedDamage(DamageType damageType, Vector3 hitPoint, Vector3 hitDirection, float hitSpeed)
    {
        enemyStats.currentHealth -= damageType.damageAmount;
        OnHealthChanged.Invoke();

        ////// Shader Impact Effect
        StartCoroutine("ImpactEffect");
        StartCoroutine("ImpactEffect");

        if (enemyStats.currentHealth <= 0)
            OnDeath(hitPoint, hitDirection, hitSpeed);
        
        if (damageType.isCrit) {
            // Play particle effect at location
        }
    }

    public void InitDamageable()
    {
        enemyStats.currentHealth = enemyStats.maxHealth;
    }

    public void OnDeath(Vector3 hitPoint, Vector3 hitDirection, float hitSpeed)
    {
        // Play cool effect on enemy
        GameObject deathEffectObject = Instantiate(deathEffectPrefab, hitPoint, Quaternion.FromToRotation(Vector3.forward, hitDirection));
        ParticleSystem.MainModule deathParticleSystem = deathEffectObject.GetComponent<ParticleSystem>().main;
        float particleLifetime = deathParticleSystem.startLifetime.constant;
        deathParticleSystem.startSpeed = hitSpeed;
        deathEffectObject.GetComponent<Renderer>().material = this.gameObject.GetComponent<Renderer>().material;
        Destroy(deathEffectObject, particleLifetime);

        // Add to player's score
        gameManager.OnAddScore.Invoke(enemySettings.traits.enemyScore, this.transform.position);

        GameObject.Destroy(this.gameObject);
        //Debug.Log($"{gameObject.name} is Dead");
    }

    void EquipWeapon() {
        equippedWeapon = this.gameObject.AddComponent<BasicWeapon>();
        
        WeaponSettings weaponSettings = gameManager.gameSettings.Weapons.Where(x => x.weaponType == enemySettings.weaponType).First();
        equippedWeapon.weaponType = weaponSettings.weaponType;
        equippedWeapon.fireType = weaponSettings.fireType;
        equippedWeapon.weaponStats = weaponSettings.stats;
        equippedWeapon.firePoint = firePoint;
        equippedWeapon.objectStats = this.enemyStats;
    }

    // Static Helper Functions
    public static float GetTargetDistance(Transform t1, Transform t2) {
        return Vector3.Magnitude(t1.position - t2.position);
    }

    public static float GetTargetDistance(Vector3 v1, Vector3 v2) {
        return Vector3.Magnitude(v1 - v2);
    }

    public static bool IsPlayerInRange(BasicEnemy enemy) {
        if (GameManager.Instance.playerController == null) return false;
        
        return Vector3.Magnitude(GameManager.Instance.playerController.transform.position - enemy.transform.position) <= enemy.enemySettings.traits.detectionRange;
    }

    ////// Methods for Shader Manipulation //////
    IEnumerator ImpactEffect()
    {
        impactMaterial.SetFloat("_Alpha_Intensity", 1f);
        float matAlpha = 1;

        while (matAlpha > 0)
        {
            matAlpha -= 0.3f;
            impactMaterial.SetFloat("_Alpha_Intensity", matAlpha);
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }
}
