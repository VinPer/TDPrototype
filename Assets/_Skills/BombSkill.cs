using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombSkill : Skill
{
    private bool isPrimed = false;
    public float timeToDetonate = 1.5f;
    public float explosionRadius = 10f;
    public float damage = 50f;
    public float piercingValue = 100f;
    public GameObject explosionEffect;

    private void Update()
    {
        if (isPrimed)
        {
            timeToDetonate -= Time.deltaTime;
        }

        if (timeToDetonate <= 0f)
        {
            Explode();
        }
    }

    protected override void ActivateSkill()
    {
        isPrimed = true;
    }

    private void Explode()
    {
        GameObject effectIns = Instantiate(explosionEffect, transform.position, transform.rotation);
        Destroy(effectIns, 5f);

        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider collider in colliders)
        {
            if (collider.tag == "Enemy")
            {
                Damage(collider.transform);
            }
        }

        Destroy(gameObject);
    }

    private void Damage(Transform enemy)
    {
        Enemy e = enemy.GetComponent<Enemy>();
        e.TakeDamage(damage, piercingValue);
    }
}
