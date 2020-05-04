using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunnerMove : MonoBehaviour
{
    public int speed;
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 localPosition = player.transform.position - transform.position;
        localPosition = localPosition.normalized;
        transform.Translate(localPosition.x * Time.deltaTime * speed, localPosition.y * Time.deltaTime * speed, localPosition.z * Time.deltaTime * speed);
    }
}
