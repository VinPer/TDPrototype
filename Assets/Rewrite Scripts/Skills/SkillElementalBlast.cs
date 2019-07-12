using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillElementalBlast : SkillGlobal
{
    public string debuffElement = "fire";
    public float debuffIntensity = 10f;
    public float debuffDuration = 5f;
    public float duration = 10f;

    private void Start()
    {
        FindTargets();
        foreach(GameObject target in enemies)
        {
            Enemy enemy = target.GetComponent<Enemy>();
            enemy.ActivateDebuff(debuffIntensity, Mathf.Infinity, debuffElement);
        }
    }

    private void Update()
    {
        duration -= Time.deltaTime;

        if (duration <= 0f)
        {
            End();
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        Enemy enemy = col.GetComponent<Enemy>();
        enemy.ActivateDebuff(debuffIntensity, Mathf.Infinity, debuffElement);
    }

    private void End()
    {
        FindTargets();
        Enemy currentEnemy;
        foreach(GameObject enemy in enemies)
        {
            currentEnemy = enemy.GetComponent<Enemy>();
            currentEnemy.ActivateDebuff(debuffIntensity, debuffDuration, debuffElement);
        }

        Destroy(gameObject);
    }
}
