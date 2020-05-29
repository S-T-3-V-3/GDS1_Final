using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System.Linq;

public class BasicPlayer : Pawn
{
    public Transform gunPosition;
    public Transform groundPosition;
    public Animator animationController;
    public LayerMask groundMask;
    public Vector3 velocity;
    public float groundDistance;
    public bool canTakeDamage = true;
    public bool isGrounded = true;

    IKWeaponsAnimator weaponsIK;
    PlayerSettings playerSettings;
    AimSystem aimSystem;

    GameManager gameManager;
    GameSettings gameSettings;
    float gravity = -9.8f;

    bool hasAim = false;

    

    void Awake()
    {
        playerSettings = GameManager.Instance.gameSettings.playerSettings;

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
        WeaponStats newStats = GameManager.Instance.gameSettings.WeaponList.Where(x => x.weaponType == WeaponType.RIFLE).First().weaponBaseStats;
        EquipWeapon<Weapon>(WeaponType.RIFLE, newStats); // TODO WE NEED TO MAKE A RIFLE WEAPON LOL
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
    public void EquipWeapon<T>(WeaponType weaponType, WeaponStats weaponStats) where T : Weapon
    {
        if (equippedWeapon != null)
            DropWeapon();

        WeaponDefinition weaponDefinition = gameSettings.WeaponList.Where(x => x.weaponType == weaponType).First();

        equippedWeapon = this.gameObject.AddComponent<T>();
        equippedWeapon.weaponStats = weaponStats;
        equippedWeapon.weaponType = weaponType;
        equippedWeapon.Init(weaponDefinition, gunPosition);

        weaponsIK.SetWeaponHandIK(equippedWeapon.weaponModel.GetComponent<WeaponTransforms>(), gunPosition);

        equippedWeapon.ownerStats = this.statHandler;
        equippedWeapon.AddShotEffect(weaponDefinition);
        equippedWeapon.canShoot = true;
    }

    void DropWeapon() { }

    public override void InitDamageable()
    {
        statHandler = gameManager.gameSettings.playerSettings.playerStats.GetCopy();
        statHandler.CurrentHealth = statHandler.MaxHealth;
        statHandler.Energy = statHandler.MaxEnergy;
    }

    public override void OnDeath(Vector3 hitPoint, Vector3 hitDirection, float hitSpeed)
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