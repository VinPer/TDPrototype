using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerRadar : TowerBase
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<SphereCollider>().radius = range;
    }

    private void OnTriggerEnter(Collider col)
    {
        col.GetComponent<EnemyBase>().UpdateInvisible(false);
    }
    private void OnTriggerExit(Collider col)
    {
        if (col.GetComponent<EnemyBase>().type == Enums.EnemyType.invisible)
            col.GetComponent<EnemyBase>().UpdateInvisible(true);
    }

    protected override void UpgradeStatus()
    {
        throw new System.NotImplementedException();
    }
}
