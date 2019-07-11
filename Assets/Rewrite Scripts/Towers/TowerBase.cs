using UnityEngine;

public abstract class TowerBase : MonoBehaviour
{
    protected enum Status { Disabled, Enabled };
    protected Status currentState = Status.Enabled;
    public float range = 15f;
    protected float cost = 100f;
    protected int level = 1;
    public bool seesInvisible = false;

    protected float rangeBoost = 1f;
    protected float rateBoost = 1f;
    protected float damageBoost = 1f;

    //public TowerModel model;

    public virtual void SetRange(float value)
    {
        if (value <= 0f) Debug.Log("Incorrect value to update range!");        
        else range = value;
    }

    public void SetCost(float value)
    {
        if (value <= 0f) Debug.Log("Incorrect value to update cost!");
        else cost = value;
    }

    public void SetPosition(Vector3 pos)
    {
        transform.position = pos;
    }

    public void SetStatus(int status)
    {
        // enum?
    }

    public void SetRangeBoost(float value)
    {
        if (value <= 0f) Debug.Log("Incorrect value to update range boost!");
        else rangeBoost = value;
    }

    public void SetRateBoost(float value)
    {
        if (value <= 0f) Debug.Log("Incorrect value to update rate boost!");
        else rateBoost = value;
    }

    public void SetDamageBoost(float value)
    {
        if (value <= 0f) Debug.Log("Incorrect value to update damage boost!");
        else damageBoost = value;
    }

    public abstract void UpgradeTower();
}
