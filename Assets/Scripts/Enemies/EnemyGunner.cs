using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemyGunner : MonoBehaviour
{
    public GameObject gunnerBulletPrefab;
    public Transform firePoint;

    Transform targetTransform;
    ObjectStats objectStats;
    EnemyStats enemyStats;
    WeaponStats standardRifleStats;

    bool hasTarget = false;
    bool isTargetInRange = false;

    private float timeSinceLastFired = 0;

    void Start()
    {
        targetTransform = GameManager.Instance.playerController.transform;
        standardRifleStats = GameManager.Instance.gameSettings.Weapons.Where(x => x.weaponName == "StandardRifle").First().Stats;
        enemyStats = GameManager.Instance.gameSettings.Enemies.Where(x => x.enemyStats.enemyName == "Gunner").First().enemyStats;
        objectStats = GameManager.Instance.gameSettings.Enemies.Where(x => x.enemyStats.enemyName == "Gunner").First().objectStats;

        this.gameObject.GetComponent<BasicEnemy>().Init(GameManager.Instance.gameSettings.Enemies.Where(x => x.enemyStats.enemyName == "Gunner").First());
    }

    void FixedUpdate()
    {
        if (hasTarget)
        {
            isTargetInRange = GetTargetDistance() <= enemyStats.detectionRange;

            if (isTargetInRange)
            {
                transform.LookAt(targetTransform);
                if (timeSinceLastFired >= objectStats.fireRate)
                {
                    ShootAtTarget();
                    timeSinceLastFired = 0;
                }
            }
        } else
        {
            SearchForTarget();
        }
        if (timeSinceLastFired < objectStats.fireRate)
            timeSinceLastFired += Time.deltaTime;
    }

    void SearchForTarget()
    {
        if (GameManager.Instance.playerController == null) return;

        targetTransform = GameManager.Instance.playerController.transform;
        hasTarget = true;
    }

    float GetTargetDistance()
    {
        return Vector3.Magnitude(this.transform.position - targetTransform.position);
    }

    void ShootAtTarget()
    {
        BasicProjectile currentBullet = GameObject.Instantiate(gunnerBulletPrefab, firePoint).GetComponent<BasicProjectile>();
        currentBullet.transform.parent = GameManager.Instance.transform;
        currentBullet.owningObject = this.gameObject;
        currentBullet.weaponStats = this.standardRifleStats;

        currentBullet.GetComponent<Rigidbody>().AddForce(this.gameObject.transform.forward * standardRifleStats.shotSpeed);

        ///////////////// kill this with fire
        GameObject.Destroy(currentBullet, 3f); // TODO
    }
}
