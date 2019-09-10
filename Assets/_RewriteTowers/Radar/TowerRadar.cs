using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TA TOSCO, AINDA TO MEXENDO
public class TowerRadar : TowerBase
{
    List<TowerBase> towersAffecting;

    // Start is called before the first frame update
    void Start()
    {
        towersAffecting = new List<TowerBase>();
        InvokeRepeating("Radar", 0f, 0.5f);
    }

    void Radar()
    {
        foreach(GameObject t in BuildManager.TurretsBuilded)
        {
            float distToTower = Vector3.Distance(transform.position, t.transform.position);
            if (distToTower <= range)
            {
                TowerBase tower = t.GetComponent<TowerBase>();
                if (!tower.seesInvisible)
                {
                    GetComponent<AudioSource>().Play();
                    tower.seesInvisible = true;
                    towersAffecting.Add(tower);
                }
            }
        }
    }

    protected override void UpgradeStatus()
    {
        if (upgrades["range"] < UpgradeHandler.data.towerUpgrades[gameObject.name]["range"])
        {
            range += rangeUpgrade;
            upgrades["range"]++;
            Debug.Log(range);
        }
    }

    private void OnDestroy()
    {
        foreach (TowerBase t in towersAffecting)
        {
            if(t != null)
                t.seesInvisible = false;
        }
    }
    private void OnDisable()
    {
        foreach (TowerBase t in towersAffecting)
        {
            if (t != null)
                t.seesInvisible = false;
        }
    }
}
