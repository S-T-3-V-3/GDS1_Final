using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHeavyGunner : MonoBehaviour
{
    Transform targetTransform;

    void Start()
    {
        targetTransform = GameManager.Instance.playerController.transform;
    }

    void Update()
    {
        transform.LookAt(targetTransform);
    }
}
