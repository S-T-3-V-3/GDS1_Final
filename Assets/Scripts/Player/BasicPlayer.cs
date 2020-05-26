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
    GameManager gameManager;
    

    void Start() {
        gameManager = GameManager.Instance;
        InitDamageable();

        // Equip basic rifle
        EquipWeapon(WeaponType.RIFLE, gameManager.gameSettings.Weapons.Where(x => x.weaponType == WeaponType.RIFLE).First().stats);
    }

    void Update() {
        //////////// Debug Input ///////////////////
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }

        if (Input.GetKey(KeyCode.Escape)){
            Application.Quit();
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
    }

    public void OnReceivedDamage(DamageType damageType)
    {
        if (canTakeDamage == false) return;

        playerStats.currentHealth -= damageType.damageAmount;
        OnHealthChanged.Invoke();

        if (playerStats.currentHealth <= 0)
            OnDeath();
        
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

    public void OnDeath()
    {
        // Play cool effect on player
        float deathAnimationSeconds = 1;
        
        gameManager.Invoke("GameOver", deathAnimationSeconds);
    }
}