using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillNuke : SkillGlobal
{
    public float damage = 100f;
    public float penetration = 100f;

    private void Start()
    {
        FindTargets();
        EnemyBase enemy;
        foreach(GameObject target in enemies)
        {
            enemy = target.GetComponent<EnemyBase>();
            enemy.TakeDamage(damage, penetration, Elements.Element.none);
        }
    }
}
