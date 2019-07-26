using UnityEngine;

public abstract class TowerNonProjectile : TowerBase
{
    public float triggerRate;
    public float damage;
    public float penetration;
    public Enums.Element debuffElement;
    public float debuffIntensity;
    public float debuffDuration;

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
