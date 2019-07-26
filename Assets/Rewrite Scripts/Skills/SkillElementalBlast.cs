using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillElementalBlast : SkillGlobal
{
    public Enums.Element debuffElement;
    public float debuffIntensity = 10f;
    public float debuffDuration = 5f;
    public float duration = 10f;

    private void Start()
    {
        FindTargets();
        foreach(GameObject target in enemies)
        {
            EnemyBase enemy = target.GetComponent<EnemyBase>();
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
        EnemyBase enemy = col.GetComponent<EnemyBase>();
        enemy.ActivateDebuff(debuffIntensity, Mathf.Infinity, debuffElement);
    }

    private void End()
    {
        FindTargets();
        EnemyBase currentEnemy;
        foreach(GameObject enemy in enemies)
        {
            currentEnemy = enemy.GetComponent<EnemyBase>();
            currentEnemy.ActivateDebuff(debuffIntensity, debuffDuration, debuffElement);
        }

        Destroy(gameObject);
    }
}
