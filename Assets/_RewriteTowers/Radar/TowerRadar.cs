using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TA TOSCO, AINDA TO MEXENDO
public class TowerRadar : TowerBase
{
    List<EnemyBase> enemiesAffecting;
    private void Start()
    {
        GetComponent<SphereCollider>().radius = range;
        enemiesAffecting = new List<EnemyBase>();
    }

    private void OnTriggerEnter(Collider col)
    {
        EnemyBase enemy = col.GetComponent<EnemyBase>();
        if(enemy != null)
        {
            if (enemy.stealth)
            {
                enemy.radarsAffecting++;
                enemy.UpdateInvisible();
                if(!enemiesAffecting.Contains(enemy))
                    enemiesAffecting.Add(enemy);
            }
        }
    }

    private void OnTriggerExit(Collider col)
    {
        EnemyBase enemy = col.GetComponent<EnemyBase>();
        if (enemy != null)
        {
            if (enemy.stealth)
            {
                enemy.radarsAffecting--;
                enemy.UpdateInvisible();
                RemoveEnemy(enemy);
            }
        }
    }

    private void OnDisable()
    {
        foreach(EnemyBase enemy in enemiesAffecting)
        {
            enemy.radarsAffecting--;
            enemy.UpdateInvisible();
            RemoveEnemy(enemy);
        }
    }

    public void RemoveEnemy(EnemyBase enemy)
    {
        if (enemiesAffecting.Contains(enemy))
            enemiesAffecting.Remove(enemy);
    }

    protected override void UpgradeStatus()
    {
        if (upgrades["range"] < UpgradeHandler.data.towerUpgrades[transform.parent.name]["range"])
        {
            range += rangeUpgrade;
            upgrades["range"]++;
            GetComponent<SphereCollider>().radius = range;
            Debug.Log(range);
        }
    }
}
