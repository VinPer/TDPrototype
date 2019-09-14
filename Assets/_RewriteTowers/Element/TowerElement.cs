using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerElement : TowerNonProjectile
{
    private void Start()
    {
        GetComponent<SphereCollider>().radius = range/3;
    }

    private void OnTriggerEnter(Collider col)
    {
        EnemyBase enemy = col.GetComponent<EnemyBase>();
        if (enemy != null)
        {
            col.GetComponent<EnemyBase>().ActivateDebuff(debuffIntensity, Mathf.Infinity, element);
            print("debuff enter");
        }
    }

    private void OnTriggerExit(Collider col)
    {
        print("hi");
        EnemyBase enemy = col.GetComponent<EnemyBase>();
        if (enemy != null)
        {
            enemy.ActivateDebuff(debuffIntensity, debuffDuration, element);
            print("debuff exit");
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
            print("range upgraded");
        }

        string _intensity = "debuffIntensity";
        if (upgrades[_intensity] < UpgradeHandler.data.towerUpgrades[transform.parent.name][_intensity])
        {
            debuffIntensity += intensityUpgrade;
            upgrades[_intensity]++;
            print("debuffIntensity upgraded");
        }

        string _duration = "debuffDuration";
        if (upgrades[_duration] < UpgradeHandler.data.towerUpgrades[transform.parent.name][_duration])
        {
            debuffDuration += durationUpgrade;
            upgrades[_duration]++;
            print("debuffDuration upgraded");
        }
    }
    protected void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
