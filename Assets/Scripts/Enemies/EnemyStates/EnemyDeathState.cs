using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeathState : EnemyState
{
    public override void BeginState() {
        Debug.Log("Enemy Died");
        GameObject.Destroy(this.gameObject);
    }
}
