using System.Collections.Generic;
using UnityEngine;

public class TowerBuff : TowerBase
{
    public float buffRange = 10f;
    public float buffRate = 10f;
    public float buffDamage = 10f;

    public float upgradeBuff = 2f;

    private List<TowerBase> towers;

    protected void Start()
    {
        buffRange = buffRange / 100;
        buffRate = buffRate / 100;
        buffDamage = buffDamage / 100;
        GetComponent<SphereCollider>().radius = range;
        towers = new List<TowerBase>();
        //InvokeRepeating("FindTowers", 0f, 0.5f);
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Enemy") || col.CompareTag("BlackHole")) return;
        print("trigger");
        TowerBase tb = col.GetComponentInChildren<TowerBase>();
        if(tb != null && !col.GetComponentInChildren<TowerBuff>())
        {
            print("add");
            towers.Add(tb);
            tb.BuffRange(buffRange);
        }
    }

    private void OnDisable()
    {
        if (towers == null) return;
        foreach (TowerBase tower in towers)
        {
            if (tower != null)
            {
                tower.BuffRange(-buffRange);
                //currentTower.SetRateBoost(-buffRate);
                //currentTower.SetDamageBoost(-buffDamage);
            }
        }
    }

    protected override void UpgradeStatus()
    {
        string _range = "range";
        if (upgrades[_range] < UpgradeHandler.data.towerUpgrades[transform.parent.name][_range])
        {
            range += rangeUpgrade;
            upgrades[_range]++;
            GetComponent<SphereCollider>().radius = range;
        }
        
        foreach (TowerBase tower in towers)
        {
            if (tower != null)
            {
                tower.BuffRange(-buffRange);
                //currentTower.SetRateBoost(-buffRate);
                //currentTower.SetDamageBoost(-buffDamage);
            }
        }


        string _buffRange = "buffRange";
        if (upgrades[_buffRange] < UpgradeHandler.data.towerUpgrades[transform.parent.name][_buffRange])
        {
            buffRange += upgradeBuff;
            upgrades[_buffRange]++;
        }

        string _buffDamage = "buffDamage";
        if (upgrades[_buffDamage] < UpgradeHandler.data.towerUpgrades[transform.parent.name][_buffDamage])
        {
            buffDamage += upgradeBuff;
            upgrades[_buffDamage]++;
        }

        string _buffFireRate = "buffFireRate";
        if (upgrades[_buffFireRate] < UpgradeHandler.data.towerUpgrades[transform.parent.name][_buffFireRate])
        {
            _buffFireRate += upgradeBuff;
            upgrades[_buffFireRate]++;
        }

        foreach (TowerBase tower in towers)
        {
            if (tower != null)
            {
                tower.BuffRange(buffRange);
                //currentTower.SetRateBoost(buffRate);
                //currentTower.SetDamageBoost(buffDamage);
            }
        }
    }
}
