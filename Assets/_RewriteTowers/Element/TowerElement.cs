using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerElement : TowerNonProjectile
{
    private List<GameObject> possibleTargets;

    private void Start()
    {
        possibleTargets = new List<GameObject>();
        GetComponent<SphereCollider>().radius = range;
        InvokeRepeating("Debuff", 0f, 0.5f);
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.GetComponent<EnemyBase>() && !possibleTargets.Contains(col.gameObject))
        {
            possibleTargets.Add(col.gameObject);
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (possibleTargets.Contains(col.gameObject))
            possibleTargets.Remove(col.gameObject);
    }

    void Debuff()
    {
        List<GameObject> backup = new List<GameObject>(possibleTargets);
        print(possibleTargets.Count);
        foreach (GameObject e in backup)
        {
            EnemyBase enemy = e.GetComponent<EnemyBase>();
            if(seesInvisible || (!seesInvisible && !enemy.GetInvisibleState()))
            {

                if (enemy.element != element)
                {
                    enemy.ActivateDebuff(debuffIntensity, debuffDuration, element);
                }
            }
        }
    }
    
    protected override void UpgradeStatus()
    {
        string _range = "range";
        if (upgrades[_range] < UpgradeHandler.data.towerUpgrades[gameObject.name][_range])
        {
            range += rangeUpgrade;
            upgrades[_range]++;
            GetComponent<SphereCollider>().radius = range;
        }

        string _intensity = "debuffIntensity";
        if (upgrades[_intensity] < UpgradeHandler.data.towerUpgrades[gameObject.name][_intensity])
        {
            debuffIntensity += intensityUpgrade;
            upgrades[_intensity]++;
        }

        string _duration = "debuffDuration";
        if (upgrades[_duration] < UpgradeHandler.data.towerUpgrades[gameObject.name][_duration])
        {
            debuffDuration += durationUpgrade;
            upgrades[_duration]++;
        }
    }
}
