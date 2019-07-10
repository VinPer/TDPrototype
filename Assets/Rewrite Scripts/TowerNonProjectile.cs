using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TowerNonProjectile : TowerBase
{
    public float triggerRate;
    public float damage;
    public float penetration;
    public string debuffElement; // change this up
    public float debuffIntensity;
    public float debuffDuration;

    public void UpdateDamage(float value)
    {
        if (value <= 0f) Debug.Log("Incorrect value to update damage!");
        else damage = value;
    }

    public void UpdateRate(float value)
    {
        if (value <= 0f) Debug.Log("Incorrect value to update trigger rate!");
        else triggerRate = value;
    }
}
