using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System.Linq;
using UnityEngine.InputSystem;

public class BasicPlayer : Pawn
{
    public Transform gunPosition;
    public Transform dropPosition;
    public Transform groundPosition;
    public Animator animationController;
    public LayerMask groundMask;
    public Vector3 velocity;
    public float groundDistance;
    public bool isGrounded = true;
    public bool isSprinting = false;
    

    IKWeaponsAnimator weaponsIK;
    PlayerSettings playerSettings;
    //AimSystem aimSystem;

    GameManager gameManager;
    GameSettings gameSettings;
    

    void Awake()
    {
        playerSettings = GameManager.Instance.gameSettings.playerSettings;
        animationController = GetComponent<Animator>();

        if (GetComponent<PlayerController>() != null)
        {
            weaponsIK = gameObject.AddComponent<IKWeaponsAnimator>();
            //aimSystem = Instantiate(playerSettings.aimSystem, transform).GetComponent<AimSystem>();
        }
    }

    void Start() {
        gameManager = GameManager.Instance;
        gameSettings = gameManager.gameSettings;
        velocity = Vector3.zero;
        InitDamageable();

        //Equip starting weapon
        WeaponType startingWeaponType = WeaponType.RIFLE;
        WeaponStats newStats = GameManager.Instance.gameSettings.WeaponList.Where(x => x.weaponType == startingWeaponType).First().weaponBaseStats;
        EquipWeapon<RifleWeapon>(startingWeaponType, newStats); // TODO WE NEED TO MAKE A RIFLE WEAPON LOL
    }

    private void Update() {
        //////////// Debug Input ///////////////////
        if (Input.GetKeyDown(KeyCode.R))
        {
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }

        if(Input.GetKeyDown(KeyCode.Semicolon)) {
            gameManager.OnAddScore.Invoke(100, Vector3.zero);
        }
        ////////////////////////////////////////////

        if (isPaused) return;

        isGrounded = Physics.CheckSphere(groundPosition.position, groundDistance, groundMask);
        
        if (isGrounded)
            velocity.y = 0;

        velocity.y += gameSettings.gravity * Time.deltaTime;
        
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
        equippedWeapon.autoAim = true;

        weaponsIK.SetWeaponHandIK(equippedWeapon.weaponModel.GetComponent<WeaponTransforms>(), gunPosition);

        equippedWeapon.ownerStats = this.statHandler;
        equippedWeapon.AddShotEffect(weaponDefinition);
        equippedWeapon.canShoot = true;

        gameManager.hud.weaponStats.Init(equippedWeapon.weaponType, equippedWeapon.weaponStats);
    }

    void DropWeapon() {

        if(equippedWeapon.weaponType == WeaponType.MELEE) return;

        RaycastHit hit;

        if(Physics.Raycast(dropPosition.position, Vector3.up * -1, out hit, 10))
        {
            Vector3 spawnPos = new Vector3(hit.point.x, hit.point.y + 1, hit.point.z);
            GameObject droppedItem = GameObject.Instantiate(GameManager.Instance.gameSettings.dropIndicator, spawnPos, Quaternion.identity);
            DroppedWeapon dropState = droppedItem.GetComponent<DroppedWeapon>();
            dropState.weaponType = equippedWeapon.weaponType;
            dropState.Init(equippedWeapon.weaponModel, "Player");
            GameObject.Destroy(equippedWeapon.weaponModel);
            GameObject.Destroy(equippedWeapon);
        }
    }

    public override void InitDamageable()
    {
        statHandler = gameManager.gameSettings.playerSettings.playerStats.GetCopy();
        statHandler.CurrentHealth = statHandler.MaxHealth;
        statHandler.Energy = statHandler.MaxEnergy;
    }

    public override void OnReceivedDamage(DamageType damageType, Vector3 hitPoint, Vector3 hitDirection, float hitSpeed){
        base.OnReceivedDamage(damageType, hitPoint, hitDirection, hitSpeed);
        
        IngameDamageText damageText = GameObject.Instantiate(gameManager.DamageTextPrefab, gameManager.transform).GetComponent<IngameDamageText>();
        damageText.damageText.text = $"{damageType.damageAmount}";
        damageText.transform.position = hitPoint;

        StartCoroutine(GameManager.Instance.hud.FadeImpact());
        AudioManager.Instance.PlaySoundEffect(SoundType.PlayerImpact);
    }

    public override void OnDeath(Vector3 hitPoint, Vector3 hitDirection, float hitSpeed)
    {
        GameObject shockwaveEffect = Instantiate(gameManager.gameSettings.ShockwavePrefab, hitPoint, Quaternion.FromToRotation(Vector3.forward, hitDirection));
        ParticleSystem.MainModule deathParticleSystem = shockwaveEffect.GetComponent<ParticleSystem>().main;
        float particleLifetime = deathParticleSystem.startLifetime.constant;
        deathParticleSystem.startSpeed = hitSpeed;
        Destroy(shockwaveEffect, particleLifetime);

        //Get player's final score
        gameManager.GameOver(particleLifetime);
        GameObject.Destroy(this.gameObject);       
    }
}