using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerProjectile : TowerBase
{
    //private Magazine magazine;
    public float fireRate = 1f;
    public Transform target;
    public int targettingStyle;


    public override void UpgradeTower()
    {
        // upgrade logic
    }
}
