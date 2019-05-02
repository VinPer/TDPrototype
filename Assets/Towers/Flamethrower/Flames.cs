using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flames : MonoBehaviour
{
    public float damage = 1f;
    public float piercingValue = 0f;
    public float damageRate = 100f;
    private float damageCountdown = 0f;
    public GameObject fireEffect;
    private GameObject flameEffect;

    // Start is called before the first frame update
    void Start()
    {
       Instantiate(fireEffect, transform);
    }

    // Damages everything within its area every 1f / damageRate seconds
    void Update()
    {
        if (damageCountdown <= 0f)
        {
            DamageArea();
            damageCountdown = 1f / damageRate;
        }

        damageCountdown -= Time.deltaTime;

    }

    // This requires optimization to the collision area
    void DamageArea()
    {
        Collider[] colliders = Physics.OverlapBox(transform.position, transform.localScale / 2, transform.rotation);
        foreach (Collider collider in colliders)
        {
            if (collider.tag == "Enemy")
            {
                Damage(collider.transform);
            }
        }
    }

    private void Damage(Transform enemy)
    {
        Enemy e = enemy.GetComponent<Enemy>();
        e.TakeDamage(damage, piercingValue);
    }
}
