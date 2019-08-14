using System.Collections.Generic;
using UnityEngine;

public class TowerBuff : TowerBase
{
    public string towerTag = "Tower";

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

        GetComponent<SphereCollider>().radius = range;
    }
    private void Update()
    {
        GetComponent<SphereCollider>().radius = range;
    }
    private void OnTriggerEnter(Collider other)
    {
        if ((other.GetComponent<TowerBase>() || other.GetComponentInChildren<TowerBase>()) && !other.GetComponentInChildren<TowerBuff>())
        {
            TowerBase currentTower;
            if (other.GetComponent<TowerBase>())
                currentTower = other.GetComponent<TowerBase>();
            else
                currentTower = other.GetComponentInChildren<TowerBase>();
            currentTower.BuffRange(buffRange);
            currentTower.SetRateBoost(buffRate);
            currentTower.SetDamageBoost(buffDamage);
            towers.Add(other.transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if ((other.GetComponent<TowerBase>() || other.GetComponentInChildren<TowerBase>()) && !other.GetComponentInChildren<TowerBuff>())
        {
            Debug.Log("eita");
            towers.Remove(other.transform);
        }

    }

    private void FindTowers()
    {
        GameObject[] turrets = GameObject.FindGameObjectsWithTag(towerTag);
        float distanceToTower;
        TowerBase currentTower;
        foreach (GameObject tower in turrets)
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
                if(tower.GetComponent<TowerBase>())
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
