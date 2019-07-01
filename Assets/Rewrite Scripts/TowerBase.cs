using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TowerBase : MonoBehaviour
{
    protected enum Status { Disabled, Enabled };
    protected Status currentState = Status.Enabled;
    public float range = 15f;
    protected float cost = 100f;
    protected int level = 1;

    //public TowerModel model;

    public void UpdateRange(float value)
    {
        if (value <= 0f || value >= 150f) Debug.Log("Incorrect value to update range!");        
        else range = value;
    }

    public void UpdateCost(float value)
    {
        if (value <= 0f) Debug.Log("Incorrect value to update cost!");
        else cost = value;
    }

    public void UpdatePosition(Vector3 pos)
    {
        transform.position = pos;
    }

    public void UpdateStatus(int status)
    {
        // enum?
    }

    public abstract void UpgradeTower();
}
