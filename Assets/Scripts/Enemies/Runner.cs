using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Runner : MonoBehaviour
{
    private GameObject player;
    EnemyType runner;

    // Start is called before the first frame update
    void Start()
    {
        runner = GameManager.Instance.gameSettings.runner;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 localPosition = player.transform.position - transform.position;
        localPosition = localPosition.normalized;
        transform.Translate(localPosition.x * Time.deltaTime * runner.enemyStats.moveSpeed, localPosition.y * Time.deltaTime * runner.enemyStats.moveSpeed, localPosition.z * Time.deltaTime * runner.enemyStats.moveSpeed);
    }
}
