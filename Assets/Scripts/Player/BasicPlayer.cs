using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System.Linq;

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
    AimSystem aimSystem;

    GameManager gameManager;
    GameSettings gameSettings;
    

    void Awake()
    {
        playerSettings = GameManager.Instance.gameSettings.playerSettings;
        animationController = GetComponent<Animator>();

        if (GetComponent<PlayerController>() != null)
        {
            weaponsIK = gameObject.AddComponent<IKWeaponsAnimator>();
            aimSystem = Instantiate(playerSettings.aimSystem, transform).GetComponent<AimSystem>();
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
        WeaponType startingWeaponType = WeaponType.SHOTGUN;
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

        if (Input.GetKey(KeyCode.Escape)){
            Application.Quit();
        }

        if(Input.GetKeyDown(KeyCode.Semicolon)) {
            gameManager.OnAddScore.Invoke(100, Vector3.zero);
        }
        ////////////////////////////////////////////

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
        //Debug.Log(weaponType);
        //Debug.Log(equippedWeapon.name);

        weaponsIK.SetWeaponHandIK(equippedWeapon.weaponModel.GetComponent<WeaponTransforms>(), gunPosition);

        equippedWeapon.ownerStats = this.statHandler;
        equippedWeapon.AddShotEffect(weaponDefinition);
        equippedWeapon.canShoot = true;
    }

    void DropWeapon() {

        if(equippedWeapon.weaponType == WeaponType.MELEE) return;

        RaycastHit hit;

        if(Physics.Raycast(dropPosition.position, Vector3.up * -1, out hit, 10))
        {
            GameObject droppedItem = GameObject.Instantiate(equippedWeapon.weaponModel, hit.point, Quaternion.identity);
            DroppedState state = droppedItem.AddComponent<DroppedState>();
            state.weaponType = equippedWeapon.weaponType;
        }
    }

    public override void InitDamageable()
    {
        statHandler = gameManager.gameSettings.playerSettings.playerStats.GetCopy();
        statHandler.CurrentHealth = statHandler.MaxHealth;
        statHandler.Energy = statHandler.MaxEnergy;
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