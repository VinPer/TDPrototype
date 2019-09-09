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
        InvokeRepeating("DebuffTargets", 0, 1f);
    }

    private void Update()
    {
        duration -= Time.deltaTime;

        if (duration <= 0f)
        {
            End();
        }
    }

    private void DebuffTargets()
    {
        FindTargets();
        foreach (GameObject target in enemies)
        {
            target.GetComponent<EnemyBase>().ActivateDebuff(debuffIntensity, debuffDuration, debuffElement);
        }
    }

    private void End()
    {
        DebuffTargets();

        Destroy(gameObject);
    }
}
