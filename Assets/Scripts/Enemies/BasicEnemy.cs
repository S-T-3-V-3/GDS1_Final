using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

public class BasicEnemy : Pawn
{
    public EnemyType enemyType;
    public Light spotLight;
    [HideInInspector] public EnemySettings enemySettings;

    Material impactMaterial;
    EnemyStateManager stateManager;
    GameManager gameManager;


    void Start()
    {
        gameManager = GameManager.Instance;

        enemySettings = gameManager.gameSettings.Enemies.Where(x => x.enemyType == this.enemyType).First();
        statHandler = enemySettings.statHandler.GetCopy();

        this.gameObject.GetComponent<Renderer>().material = enemySettings.traits.material;

        // Get Impact Material
        impactMaterial = GetComponent<MeshRenderer>().materials[1];

        if (enemySettings.weaponType != WeaponType.MELEE)
            EquipWeapon();

        stateManager = this.gameObject.AddComponent<EnemyStateManager>();
        SetState<EnemySpawnState>();

        // Test example of giving enemies points into random stats,
        // can obviously skew this based on enemy type later via
        // enemy prefabs by setting weights towards certain stats
        int numRandomStats = 3;
        while (numRandomStats > 0) {
            statHandler.LevelUp((StatType)Random.Range(1,7));
            numRandomStats--;
        }
        //

        InitDamageable();
    }

    public void SetState<T>() where T : EnemyState
    {
        stateManager.AddState<T>();
    }

    public override void InitDamageable()
    {

    }

    public override void OnReceivedDamage(DamageType damageType, Vector3 hitPoint, Vector3 hitDirection, float hitSpeed)
    {
        base.OnReceivedDamage(damageType, hitPoint, hitDirection, hitSpeed);

        GameObject debryEffect = Instantiate(GameManager.Instance.gameSettings.debrySparkEffect, hitPoint, Quaternion.identity);
        GameObject.Destroy(debryEffect, 2f);
    }

    public override void OnDeath(Vector3 hitPoint, Vector3 hitDirection, float hitSpeed)
    {
        // Play cool effect on enemy
        GameObject deathEffectObject = Instantiate(deathEffectPrefab, hitPoint, Quaternion.FromToRotation(Vector3.forward, hitDirection));
        ParticleSystem.MainModule deathParticleSystem = deathEffectObject.GetComponent<ParticleSystem>().main;
        float particleLifetime = deathParticleSystem.startLifetime.constant;
        deathParticleSystem.startSpeed = hitSpeed;
        deathEffectObject.GetComponent<Renderer>().material = this.gameObject.GetComponent<Renderer>().material;
        Destroy(deathEffectObject, particleLifetime);

        GameObject experienceEffect = GameObject.Instantiate(gameManager.gameSettings.experienceOrbEffect, transform.position, Quaternion.identity);
        GameObject.Destroy(experienceEffect, 6f);

        GameObject shockWave = GameObject.Instantiate(gameManager.gameSettings.shockwaveEffect, transform.position, Quaternion.identity);
        GameObject.Destroy(shockWave, 3);
        // Add to player's score
        gameManager.OnAddScore.Invoke(enemySettings.traits.enemyScore, this.transform.position);

        GameObject.Destroy(this.gameObject);
        //Debug.Log($"{gameObject.name} is Dead");
    }

    void EquipWeapon() {
        equippedWeapon = this.gameObject.AddComponent<Weapon>();
        
        WeaponDefinition weaponSettings = gameManager.gameSettings.WeaponList.Where(x => x.weaponType == enemySettings.weaponType).First();
        equippedWeapon.canShoot = true;
        equippedWeapon.weaponType = weaponSettings.weaponType;
        equippedWeapon.weaponStats = weaponSettings.weaponBaseStats;
        equippedWeapon.firePoint = firePoint;
        equippedWeapon.ownerStats = this.statHandler;
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
