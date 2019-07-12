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
        Enemy enemy;
        foreach(GameObject target in enemies)
        {
            enemy = target.GetComponent<Enemy>();
            enemy.TakeDamage(damage, penetration);
        }
    }
}
