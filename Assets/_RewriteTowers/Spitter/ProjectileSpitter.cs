using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpitter : ProjectileBase
{
    public GameObject acidPuddle;
    private GameObject puddle;

    protected override void Awake()
    {
        base.Awake();
        Vector3 t = new Vector3(transform.position.x, transform.position.y - 1.5f, transform.position.z);
        puddle = Instantiate(acidPuddle, t, Quaternion.identity);
        puddle.SetActive(false);
    }

    protected override void Hit(Transform hitPart)
    {
        base.Hit(hitPart);
        puddle.GetComponent<Puddle>().duration = GetComponentInParent<TowerProjectile>().puddleDuration;
        puddle.transform.SetParent(transform.parent);
        puddle.transform.position = transform.position;
        puddle.SetActive(true);
    }
}
