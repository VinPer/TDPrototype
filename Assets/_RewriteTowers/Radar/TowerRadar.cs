using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TA TOSCO, AINDA TO MEXENDO
public class TowerRadar : TowerBase
{
    List<EnemyBase> enemies;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<SphereCollider>().radius = range;
        enemies = new List<EnemyBase>();
    }

    private void OnTriggerEnter(Collider col)
    {
        col.GetComponent<EnemyBase>().UpdateInvisible(false);
        col.GetComponent<EnemyBase>().radarsAffecting++;
        enemies.Add(col.GetComponent<EnemyBase>());
    }
    private void OnTriggerExit(Collider col)
    {
        col.GetComponent<EnemyBase>().radarsAffecting--;
        enemies.Remove(col.GetComponent<EnemyBase>());
        if (col.GetComponent<EnemyBase>().type == Enums.EnemyType.invisible && col.GetComponent<EnemyBase>().radarsAffecting <= 0)
        {
            col.GetComponent<EnemyBase>().UpdateInvisible(true);
        }
    }
    protected override void UpgradeStatus()
    {
        throw new System.NotImplementedException();
    }
    private void OnDestroy()
    {
        foreach(EnemyBase enemy in enemies)
        {
            enemy.radarsAffecting--;
            if (enemy.type == Enums.EnemyType.invisible && enemy.radarsAffecting <= 0)
            {
                enemy.UpdateInvisible(true);
            }
        }
    }

}
