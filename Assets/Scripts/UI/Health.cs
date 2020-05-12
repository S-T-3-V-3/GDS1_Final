using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;

    //= delegate to avoid null reference exception
    public Action<float> OnHealthPercentChange = delegate { };

    void Start()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        //For testing purposes, we're using space to modify health
        if (Input.GetKeyDown(KeyCode.Space))
            ModifyHealth(-10);
    }

    //Add or subrtract current health, update the percent
    public void ModifyHealth(int amount)
    {
        currentHealth += amount;

        float currentHealthPercent = (float)currentHealth / (float)maxHealth;
        OnHealthPercentChange(currentHealthPercent);
    }
    
}
