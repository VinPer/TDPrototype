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

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void UpgradeStatus()
    {
        throw new System.NotImplementedException();
    }
}
