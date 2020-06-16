using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Weapon : MonoBehaviour
{
    public GameObject weaponModel;
    public WeaponType weaponType;
    public StatHandler ownerStats;
    public WeaponStats weaponStats;
    public Transform firePoint;
    public ParticleSystem muzzleFlash;

    [HideInInspector] public RaycastHit hit;
    [HideInInspector] public Vector3 rayDirection;
    [HideInInspector] public Vector3 offset;
    [HideInInspector] public bool canShoot = false;

    public virtual void Init(WeaponDefinition weaponDefinition, Transform gunPosition) {
        weaponModel = GameObject.Instantiate(weaponDefinition.WeaponPrefab);
        weaponModel.transform.parent = gunPosition;
        weaponModel.transform.position = gunPosition.position;
        
        firePoint = weaponModel.GetComponent<WeaponTransforms>().firePoint;
    }

    public virtual void Shoot() { }

    public virtual void AddShotEffect(WeaponDefinition settings) { }

    public IEnumerator Reload()
    {
        canShoot = false;
        float timeSinceFired = 0;

        while (timeSinceFired < (1/ownerStats.AttackSpeed))
        {
            yield return null;
            if (GameManager.Instance.sessionData.isPaused) yield return null;
            timeSinceFired += Time.deltaTime;
        }

        canShoot = true;
    }
}