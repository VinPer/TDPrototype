using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpitter : ProjectileBase
{
    public GameObject acidPuddle;

    protected override void Hit(Transform hitPart)
    {
        base.Hit(hitPart);
        Vector3 t = new Vector3(transform.position.x,transform.position.y -1.5f,transform.position.z);
        GameObject p = Instantiate(acidPuddle,t,Quaternion.identity);
        p.transform.SetParent(transform.parent);
    }
}
