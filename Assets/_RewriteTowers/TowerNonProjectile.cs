﻿using UnityEngine;

public abstract class TowerNonProjectile : TowerBase
{
    public float triggerRate;
    public float damage;
    public float damageUpgrade = 2f;
    public float penetration;
    public float debuffIntensity;
    public float intensityUpgrade = 5f;
    public float debuffDuration;
    public float durationUpgrade = 1f;

    public void SetDamage(float value)
    {
        if (value <= 0f) Debug.Log("Incorrect value to update damage!");
        else damage = value;
    }

    public void SetRate(float value)
    {
        if (value <= 0f) Debug.Log("Incorrect value to update trigger rate!");
        else triggerRate = value;
    }
}
