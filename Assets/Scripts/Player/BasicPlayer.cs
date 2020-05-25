using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System.Linq;

public class BasicPlayer : MonoBehaviour, IDamageable
{
    public Transform firePoint;
    public StatHandler statHandler;
    public Transform gunPosition;
    public BasicWeapon equippedWeapon;
    public bool canTakeDamage = true;
    Material impactMaterial;
    GameManager gameManager;
    GameSettings gameSettings;

    public GameObject deathEffectPrefab;
    

    void Start() {
        gameManager = GameManager.Instance;
        gameSettings = gameManager.gameSettings;
        equippedWeapon = this.gameObject.AddComponent<BasicWeapon>();
        InitDamageable();

        // Get Impact Material
        //impactMaterial = GetComponent<MeshRenderer>().materials[1];

        //Equip starting weapon
        SwitchWeapons(WeaponType.RIFLE);
    }

    void Update() {
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
    }

    public void EquipWeapon(WeaponType weaponType, WeaponStats weaponStats) {
        if (equippedWeapon != null) {
            // TODO: Drop existing weapoon
        }

        equippedWeapon = this.gameObject.AddComponent<BasicWeapon>();
        
        WeaponSettings weaponSettings = gameManager.gameSettings.Weapons.Where(x => x.weaponType == weaponType).First();
        equippedWeapon.weaponType = weaponType;
        equippedWeapon.fireType = weaponSettings.fireType;
        equippedWeapon.weaponStats = weaponStats;
        equippedWeapon.firePoint = firePoint;
        equippedWeapon.ownerStats = this.statHandler;
    }

    //SWITCHES WEAPONS
    public void SwitchWeapons(WeaponType weaponType)
    {
        WeaponStats weaponStats;
        WeaponSettings weaponSettings;

        weaponSettings = gameSettings.Weapons.Where(x => x.weaponType == weaponType).First();
        weaponStats = weaponSettings.stats;

        equippedWeapon.ChangeWeapons(gunPosition, this.statHandler, weaponType, weaponStats, weaponSettings);
    }

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
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        //Get player's final score
        gameManager.GameOver(particleLifetime);
        GameObject.Destroy(this.gameObject);       
    }

    //NEEDS UPDATING
    IEnumerator ImpactEffect()
    {
        impactMaterial.SetFloat("_Alpha_Intensity", 1f);
        float matAlpha = 1;

        while(matAlpha > 0)
        {
            matAlpha -= 0.2f;
            impactMaterial.SetFloat("_Alpha_Intensity", matAlpha);
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }
}