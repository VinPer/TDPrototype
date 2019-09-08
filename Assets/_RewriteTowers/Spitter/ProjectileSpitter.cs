using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpitter : ProjectileBase
{
    public GameObject acidPuddle;

    protected override void Hit(Transform hitPart)
    {
        base.Hit(hitPart);
        Vector3 t = new Vector3(transform.position.x,transform.position.y -2f,transform.position.z);
        GameObject p = Instantiate(acidPuddle,t,transform.rotation);
        p.transform.SetParent(transform.parent);
    }   
}
