using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System.Linq;

public class BasicPlayer : MonoBehaviour, IDamageable
{
    public Transform firePoint;
    public ObjectStats playerStats;
    public UnityEvent OnHealthChanged;
    public BasicWeapon equippedWeapon;
    public bool canTakeDamage = true;
    Material impactMaterial;
    GameManager gameManager;

    public GameObject deathEffectPrefab;
    

    void Start() {
        gameManager = GameManager.Instance;
        InitDamageable();

        // Get Impact Material
        impactMaterial = GetComponent<MeshRenderer>().materials[1];

        // Equip basic rifle
        EquipWeapon(WeaponType.RIFLE, gameManager.gameSettings.Weapons.Where(x => x.weaponType == WeaponType.RIFLE).First().stats);
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

    // TODO: Move level up values to a static level up handler
    public void LevelUp(string currentStat) {
        switch(currentStat) {
            case "maxHP":
                playerStats.maxHealth = playerStats.maxHealth + 10f;
                playerStats.currentHealth += 10f;
                OnHealthChanged.Invoke();
                break;
            case "HPRegen":
                playerStats.healthRegenSpeed = playerStats.healthRegenSpeed + 1.5f;
                break;
            case "speed":
                playerStats.moveSpeed = playerStats.moveSpeed + 3f;
                break;
            case "damage":
                playerStats.damage = playerStats.damage + 5f;
                break;
            case "fireRate":
                playerStats.fireRate = playerStats.fireRate * 0.90f;
                break;
            default:
                break;
        }

        equippedWeapon.objectStats = this.playerStats;
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
        equippedWeapon.objectStats = this.playerStats;
    }

    public void OnReceivedDamage(DamageType damageType, Vector3 hitPoint, Vector3 hitDirection, float hitSpeed)
    {
        if (canTakeDamage == false) return;

        playerStats.currentHealth -= damageType.damageAmount;
        OnHealthChanged.Invoke();
        StartCoroutine("ImpactEffect");
        StartCoroutine("ImpactEffect");

        if (playerStats.currentHealth <= 0)
            OnDeath(hitPoint, hitDirection, hitSpeed);
        
        if (damageType.isCrit) {
            // Play particle effect at location
        }
    }

    public void InitDamageable()
    {
        playerStats = gameManager.gameSettings.playerSettings.baseStats;
        playerStats.currentHealth = playerStats.maxHealth;
        playerStats.currentStamina = playerStats.maxStamina;
    }

    public void OnDeath(Vector3 hitPoint, Vector3 hitDirection, float hitSpeed)
    {
        // Play cool effect on player
        GameObject deathEffectObject = Instantiate(deathEffectPrefab, hitPoint, Quaternion.FromToRotation(Vector3.forward, hitDirection));
        ParticleSystem.MainModule deathParticleSystem = deathEffectObject.GetComponent<ParticleSystem>().main;
        float particleLifetime = deathParticleSystem.startLifetime.constant;
        deathParticleSystem.startSpeed = hitSpeed;
        Destroy(deathEffectObject, particleLifetime);

        //Get player's final score

        gameManager.GameOver(particleLifetime);
        GameObject.Destroy(this.gameObject);       
    }

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