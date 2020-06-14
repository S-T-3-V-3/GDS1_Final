using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

public class BasicEnemy : Pawn
{
    public bool debug = false;
    public EnemyType enemyType;
    public Transform weaponPosition;
    public Light spotLight;
    
    [Space]
    [Header("Custom Physics")]
    public Transform groundPosition;
    public LayerMask groundMask;
    public Vector3 velocity;
    public float groundDistance;
    public bool isGrounded = true;
    [Space]

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

        WeaponStats weaponStats = gameManager.gameSettings.WeaponList.Where(x => x.weaponType == enemySettings.weaponType).First().weaponBaseStats;

        EnemyWeaponHandler.AddWeapon(this, weaponStats);

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

    public void GravityUpdate() {
        isGrounded = Physics.CheckSphere(groundPosition.position, groundDistance, groundMask);
        
        if (isGrounded)
            velocity.y = 0;
        else {
            if (debug)
                Debug.Log("Airborne");
        }

        velocity.y += gameManager.gameSettings.gravity * Time.deltaTime;
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

        GameObject debryEffect = Instantiate(GameManager.Instance.gameSettings.DebrisSparkPrefab, hitPoint, Quaternion.identity);
        GameObject.Destroy(debryEffect, 2f);
    }

    public override void OnDeath(Vector3 hitPoint, Vector3 hitDirection, float hitSpeed)
    {
        // Play cool effect on enemy
        GameObject deathEffectObject = Instantiate(gameManager.gameSettings.DirectionalExplosionPrefab, hitPoint, Quaternion.FromToRotation(Vector3.forward, hitDirection));
        ParticleSystem.MainModule deathParticleSystem = deathEffectObject.GetComponent<ParticleSystem>().main;
        float particleLifetime = deathParticleSystem.startLifetime.constant;
        deathParticleSystem.startSpeed = hitSpeed;
        deathEffectObject.GetComponent<Renderer>().material = this.gameObject.GetComponent<Renderer>().material;
        GameObject.Destroy(deathEffectObject, particleLifetime);

        GameObject experienceEffect = GameObject.Instantiate(gameManager.gameSettings.ExperienceOrbPrefab, transform.position, Quaternion.identity);
        GameObject.Destroy(experienceEffect, 6f);

        GameObject shockWave = GameObject.Instantiate(gameManager.gameSettings.ShockwavePrefab, transform.position, Quaternion.identity);
        GameObject.Destroy(shockWave, 3);
        // Add to player's score
        gameManager.OnAddScore.Invoke(enemySettings.traits.enemyScore, this.transform.position);

        DropWeapon();
        GameObject.Destroy(this.gameObject);
        //Debug.Log($"{gameObject.name} is Dead");
    }

    public void EquipWeapon<T>(WeaponType weaponType, WeaponStats weaponStats) where T : Weapon
    {
        WeaponDefinition weaponDefinition = gameManager.gameSettings.WeaponList.Where(x => x.weaponType == weaponType).First();

        equippedWeapon = this.gameObject.AddComponent<T>();
        equippedWeapon.weaponStats = weaponStats;
        equippedWeapon.weaponType = weaponType;
        equippedWeapon.Init(weaponDefinition, weaponPosition);
        //Debug.Log(weaponType);
        //Debug.Log(equippedWeapon.name);

        equippedWeapon.ownerStats = this.statHandler;
        equippedWeapon.AddShotEffect(weaponDefinition);
        equippedWeapon.canShoot = true;
        this.firePoint = equippedWeapon.firePoint;
    }

    void DropWeapon()
    {
        if (equippedWeapon == null) return;
        if (equippedWeapon.weaponModel == null || equippedWeapon.weaponType == WeaponType.MELEE) return;

        RaycastHit hit;

        if (Physics.Raycast(transform.position, Vector3.up * -1, out hit, 10))
        {
            ///CHANGE THIS TO A BETTER VERSION WHEN THE WEAPONS ARE WORKING FOR ENEMY

            //WeaponDefinition weaponSettings = gameManager.gameSettings.WeaponList.Where(x => x.weaponType == enemySettings.weaponType).First();
            GameObject droppedItem = GameObject.Instantiate(equippedWeapon.weaponModel, hit.point, Quaternion.identity);
            DroppedState item = droppedItem.AddComponent<DroppedState>();

            ////THIS IS TEMPORARY
            ///UNTIL WEAPON IS FIXED TODO IMPLEMENT THE EQUIPPED WEAPON TYPE
            ///
            int randomSelect = Random.Range(0, 10);

            if (randomSelect > 6)
                item.weaponType = WeaponType.RIFLE;
            else
                item.weaponType = WeaponType.SHOTGUN;
        }
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

    public static bool IsPlayerInWeaponRange(BasicEnemy enemy) {        
        if (GameManager.Instance.playerController == null) return false;
        return Vector3.Magnitude(GameManager.Instance.playerController.transform.position - enemy.transform.position) <= enemy.equippedWeapon.weaponStats.range;
    }

    private void OnParticleCollision(GameObject other)
    {

        if (other.name.Contains("Shotgun_Particles")) {
            ParticleSystem shotgunParticles = other.GetComponent<ParticleSystem>();

            //MUST BE OPTIMISED
            WeaponDefinition weaponDefinition = gameManager.gameSettings.WeaponList.Where(x => x.weaponType == WeaponType.SHOTGUN).First();

            DamageType damage;
            damage.owningObject = this.gameObject;
            damage.impactPosition = other.transform.position;
            damage.impactVelocity = shotgunParticles.main.startSpeed.constant * other.transform.forward;
            damage.damageAmount = weaponDefinition.weaponBaseStats.weaponDamage;
            damage.isCrit = false;
            damage.isPiercing = false;

            OnReceivedDamage(damage, other.transform.position, other.transform.forward, shotgunParticles.main.startSpeed.constant);
        }

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

public static class EnemyWeaponHandler {
    public static void AddWeapon(BasicEnemy enemy, WeaponStats weaponStats) {
        if (enemy.enemySettings.weaponType != WeaponType.MELEE) {
            switch (enemy.enemySettings.weaponType) {
                case WeaponType.RIFLE:
                    enemy.EquipWeapon<RifleWeapon>(enemy.enemySettings.weaponType, weaponStats);
                    break;

                case WeaponType.SHOTGUN:
                    enemy.EquipWeapon<ShotgunWeapon>(enemy.enemySettings.weaponType, weaponStats);
                    break;

                case WeaponType.MACHINE_GUN:
                    //enemy.EquipWeapon<MachineGunWeapon>(enemy.enemySettings.weaponType, weaponStats);
                    break;

                case WeaponType.LASER:
                    enemy.EquipWeapon<LaserWeapon>(enemy.enemySettings.weaponType, weaponStats);
                    break;

                default:
                    break;
            }
        }
    }
}