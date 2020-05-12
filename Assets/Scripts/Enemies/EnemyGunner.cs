using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemyGunner : MonoBehaviour
{
    Transform targetTransform;
    EnemyStats gunnerStats;

    public GameObject gunnerBulletPrefab;

    WeaponStats standardRifleStats;

    bool hasTarget = false;
    bool isTargetInRange = false;
    private float timeSinceLastFired = 0;

    void Start()
    {
        targetTransform = GameManager.Instance.playerController.transform;
        standardRifleStats = GameManager.Instance.gameSettings.Weapons.Where(x => x.weaponName == "StandardRifle").First().Stats;
        gunnerStats = GameManager.Instance.gameSettings.Enemies.Where(x => x.EnemyName == "Gunner").First().enemyStats;
    }

    void FixedUpdate()
    {
        if (hasTarget)
        {
            isTargetInRange = GetTargetDistance() <= gunnerStats.detectionRange;

            if (isTargetInRange)
            {
                transform.LookAt(targetTransform);
                if (timeSinceLastFired >= gunnerStats.fireRate)
                {
                    ShootAtTarget();
                    timeSinceLastFired = 0;
                }
            }
        } else
        {
            SearchForTarget();
        }
        if (timeSinceLastFired < gunnerStats.fireRate)
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
        GameObject currentBullet = GameObject.Instantiate(gunnerBulletPrefab, transform.position, transform.rotation);
        currentBullet.GetComponent<Rigidbody>().AddForce(this.gameObject.transform.forward * standardRifleStats.shotSpeed);
        GameObject.Destroy(currentBullet, 3f);
    }
}
