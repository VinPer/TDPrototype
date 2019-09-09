using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuddleSkill : Skill
{
    public float debuffIntensity = 10f;
    public float debuffDuration = 5f;
    public string debuffType = "fire";
    public float duration = 10f;

    public string enemyTag = "Enemy";

    private bool placed = false;
    private bool disappear = false;

    protected override void ActivateSkill()
    {
        placed = true;
    }

    private void Update()
    {
        if (!placed) return;
        duration -= Time.deltaTime;

        if (duration <= 0f && !disappear)
        {
            disappear = true;
            Collider[] colliders = Physics.OverlapSphere(transform.position, 2.5f);
            foreach(Collider col in colliders)
            {
                if(col.CompareTag(enemyTag))
                {
                    Enemy e = col.GetComponent<Enemy>();
                    e.ActivateDebuff(debuffIntensity, debuffDuration, debuffType);
                }
            }
            Destroy(gameObject);
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
}
