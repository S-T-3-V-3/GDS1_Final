using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGunner : MonoBehaviour
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
