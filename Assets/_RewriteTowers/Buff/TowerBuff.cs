using System.Collections.Generic;
using UnityEngine;

public class TowerBuff : TowerBase
{
    public float buffRange = 10f;
    public float buffRate = 10f;
    public float buffDamage = 10f;

    public float upgradeBuff = 2f;

    private List<Transform> towers;

    protected void Start()
    {
        buffRange = buffRange / 100;
        buffRate = buffRate / 100;
        buffDamage = buffDamage / 100;

        towers = new List<Transform>();
        InvokeRepeating("FindTowers", 0f, 0.5f);
    }


    private void FindTowers()
    {
        float distanceToTower;
        TowerBase currentTower;
        foreach (GameObject tower in BuildManager.TurretsBuilded)
        {
            distanceToTower = Vector3.Distance(transform.position, tower.transform.position);
            if (distanceToTower <= range && !towers.Contains(tower.transform))
            {
                towers.Add(tower.transform);
                if (tower.GetComponent<TowerBase>())
                    currentTower = tower.GetComponent<TowerBase>();
                else
                    currentTower = tower.GetComponentInChildren<TowerBase>();
                currentTower.BuffRange(buffRange);
                currentTower.SetRateBoost(buffRate);
                currentTower.SetDamageBoost(buffDamage);
            }
        }
    }

    private void OnDestroy()
    {
        TowerBase currentTower;
        foreach (Transform tower in towers)
        {
            if (tower != null)
            {
                if (tower.GetComponent<TowerBase>())
                    currentTower = tower.GetComponent<TowerBase>();
                else
                    currentTower = tower.GetComponentInChildren<TowerBase>();
                currentTower.BuffRange(-buffRange);
                currentTower.SetRateBoost(-buffRate);
                currentTower.SetDamageBoost(-buffDamage);
            }
        }
    }

    protected override void UpgradeStatus()
    {
        range += rangeUpgrade;
        GetComponent<SphereCollider>().radius = range;

        TowerBase currentTower;
        foreach (Transform tower in towers)
        {
            if (tower != null)
            {
                Debug.Log("oi");
                if (tower.GetComponent<TowerBase>())
                    currentTower = tower.GetComponent<TowerBase>();
                else
                    currentTower = tower.GetComponentInChildren<TowerBase>();
                currentTower.BuffRange(-buffRange);
                currentTower.SetRateBoost(-buffRate);
                currentTower.SetDamageBoost(-buffDamage);
            }
        }

        buffDamage += upgradeBuff;
        buffDamage += upgradeBuff;
        buffRate += upgradeBuff;

        foreach (Transform tower in towers)
        {
            if (tower != null)
            {
                Debug.Log("oi");
                if (tower.GetComponent<TowerBase>())
                    currentTower = tower.GetComponent<TowerBase>();
                else
                    currentTower = tower.GetComponentInChildren<TowerBase>();
                currentTower.BuffRange(buffRange);
                currentTower.SetRateBoost(buffRate);
                currentTower.SetDamageBoost(buffDamage);
            }
        }
    }
}
