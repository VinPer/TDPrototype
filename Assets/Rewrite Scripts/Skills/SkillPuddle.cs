using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillPuddle : SkillTargetted
{
    public float debuffIntensity = 10f;
    public float debuffDuration = 5f;
    public string debuffType = "fire";
    public float duration = 10f;

    public string enemyTag = "Enemy";
    
    private bool disappear = false;

    private void Update()
    {
        if (!placed) return;
        duration -= Time.deltaTime;

        if (duration <= 0f && !disappear)
        {
            Disappear();
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.tag != enemyTag || !placed || disappear) return;

        Enemy e = col.GetComponent<Enemy>();
        e.ActivateDebuff(debuffIntensity, Mathf.Infinity, debuffType);
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.tag != enemyTag || placed == false) return;

        Enemy e = col.GetComponent<Enemy>();
        e.ActivateDebuff(debuffIntensity, debuffDuration, debuffType);
    }

    private void Disappear()
    {
        disappear = true;
        Collider[] colliders = Physics.OverlapSphere(transform.position, 2.5f);
        foreach (Collider col in colliders)
        {
            if (col.tag == enemyTag)
            {
                Enemy e = col.GetComponent<Enemy>();
                e.ActivateDebuff(debuffIntensity, debuffDuration, debuffType);
            }
        }
        Destroy(gameObject);
    }
}
