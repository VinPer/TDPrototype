using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerElement : TowerBase
{
    public float amount = 10f;

    private void Start()
    {
        InvokeRepeating("Debuff", 0f, 0.5f);
    }

    void Debuff()
    {
        foreach (GameObject e in WaveSpawner.EnemiesAlive)
        {
            EnemyBase enemy = e.GetComponent<EnemyBase>();
            if(seesInvisible || (!seesInvisible && !enemy.GetInvisibleState()))
            {

                if (enemy.element != element)
                {
                    float distToEnemy = Vector3.Distance(transform.position, e.transform.position);
                    if (distToEnemy <= range)
                    {
                        enemy.ActivateDebuff(amount, 5f, element);

                    }
                }
            }
        }
    }
    
    protected override void UpgradeStatus()
    {
        range += rangeUpgrade;
    }
}
