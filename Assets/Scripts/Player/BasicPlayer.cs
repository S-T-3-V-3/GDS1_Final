using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System.Linq;

public class BasicPlayer : MonoBehaviour, IDamageable
{
    //public Transform firePoint;
    public StatHandler statHandler;
    public Transform gunPosition;
    public Transform groundPosition;
    public BasicWeapon equippedWeapon;
    public Animator animationController;
    public LayerMask groundMask;
    public Vector3 velocity;
    public float groundDistance;
    public bool canTakeDamage = true;
    public bool isGrounded = true;

    IKWeaponsAnimator weaponsIK;
    PlayerSettings playerSettings;
    AimSystem aimSystem;

    //Material impactMaterial;
    GameManager gameManager;
    GameSettings gameSettings;
    float gravity = -9.8f;

    bool hasAim = false;

    public GameObject deathEffectPrefab;

    void Awake()
    {
        playerSettings = GameManager.Instance.gameSettings.playerSettings;
        animationController = GetComponent<Animator>();

        if (GetComponent<PlayerController>() != null)
        {
            weaponsIK = gameObject.AddComponent<IKWeaponsAnimator>();
            aimSystem = Instantiate(playerSettings.aimSystem, transform).GetComponent<AimSystem>();
            hasAim = true;
        }
    }

    void Start() {
        gameManager = GameManager.Instance;
        gameSettings = gameManager.gameSettings;
        velocity = Vector3.zero;
        InitDamageable();

        // Get Impact Material
        //impactMaterial = GetComponent<MeshRenderer>().materials[1];

        //Equip starting weapon
        EquipWeapon(WeaponType.RIFLE);
    }

    private void Update() {
        //////////// Debug Input ///////////////////
        if (Input.GetKeyDown(KeyCode.R))
        {
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }

        if (Input.GetKey(KeyCode.Escape)){
            Application.Quit();
        }

        if(Input.GetKeyDown(KeyCode.I)) {
            gameManager.OnAddScore.Invoke(100, Vector3.zero);
        }
        ////////////////////////////////////////////

        isGrounded = Physics.CheckSphere(groundPosition.position, groundDistance, groundMask);
        
        if (isGrounded)
            velocity.y = 0;

        velocity.y += gravity * Time.deltaTime;
    }

    //EQUIPS WEAPONS
    public void EquipWeapon(WeaponType weaponType)
    {
        if (equippedWeapon != null)
        {
            if (equippedWeapon.weaponType != weaponType) DropWeapon();
        }
        
        switch (weaponType)
        {
            case WeaponType.RIFLE:
                equippedWeapon = this.gameObject.AddComponent<BasicWeapon>();
                break;
            case WeaponType.SHOTGUN:
                equippedWeapon = this.gameObject.AddComponent<ShotgunWeapon>();
                break;
            case WeaponType.LASER:
                equippedWeapon = this.gameObject.AddComponent<LaserWeapon>();
                break;
        }

        WeaponSettings weaponSettings = gameSettings.Weapons.Where(x => x.weaponType == weaponType).First();
        WeaponStats weaponStats = weaponSettings.stats;
        WeaponItem weaponInstance = Instantiate(weaponSettings.weaponPrefab, gunPosition).GetComponent<WeaponItem>();

        weaponsIK.SetWeaponHandIK(weaponInstance, gunPosition);

        equippedWeapon.weaponStats = weaponStats;
        equippedWeapon.fireType = weaponSettings.fireType;
        equippedWeapon.weaponType = weaponType;
        equippedWeapon.firePoint = weaponInstance.firePoint;
        equippedWeapon.ownerStats = this.statHandler;
        
        equippedWeapon.AddShotEffect(weaponSettings);
        equippedWeapon.canShoot = true;
    }

    void DropWeapon() { }

    public void OnReceivedDamage(DamageType damageType, Vector3 hitPoint, Vector3 hitDirection, float hitSpeed)
    {
        if (canTakeDamage == false) return;

        statHandler.CurrentHealth -= damageType.damageAmount;
        //StartCoroutine("ImpactEffect");
        //StartCoroutine("ImpactEffect");

        if (statHandler.CurrentHealth <= 0)
            OnDeath(hitPoint, hitDirection, hitSpeed);
        
        if (damageType.isCrit) {
            // Play particle effect at location
        }
    }

    public void InitDamageable()
    {
        statHandler = gameManager.gameSettings.playerSettings.playerStats.GetCopy();
        statHandler.CurrentHealth = statHandler.MaxHealth;
        statHandler.CurrentStamina = statHandler.Agility.maxStamina;
    }

    public void OnDeath(Vector3 hitPoint, Vector3 hitDirection, float hitSpeed)
    {
        // Play cool effect on player
        GameObject deathEffectObject = Instantiate(deathEffectPrefab, hitPoint, Quaternion.FromToRotation(Vector3.forward, hitDirection));
        ParticleSystem.MainModule deathParticleSystem = deathEffectObject.GetComponent<ParticleSystem>().main;
        float particleLifetime = deathParticleSystem.startLifetime.constant;
        deathParticleSystem.startSpeed = hitSpeed;
        Destroy(deathEffectObject, particleLifetime);

        //Restore Cursor
        //Cursor.visible = true;
        //Cursor.lockState = CursorLockMode.None;

        //Get player's final score
        gameManager.GameOver(particleLifetime);
        GameObject.Destroy(this.gameObject);       
    }

    //NEEDS UPDATING
    /*IEnumerator ImpactEffect()
    {
        impactMaterial.SetFloat("_Alpha_Intensity", 1f);
        float matAlpha = 1;

        while(matAlpha > 0)
        {
            matAlpha -= 0.2f;
            impactMaterial.SetFloat("_Alpha_Intensity", matAlpha);
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }*/
}