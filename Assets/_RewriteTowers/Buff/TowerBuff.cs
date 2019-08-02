using System.Collections.Generic;
using UnityEngine;

public class TowerBuff : TowerBase
{
    public string towerTag = "Tower";

    public float buffRange = 10f;
    public float buffRate = 10f;
    public float buffDamage = 10f;

    private List<Transform> towers;

    protected override void Start()
    {
        base.Start();

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
        if (other.GetComponent<TowerBase>() && !other.GetComponent<TowerBuff>())
        {
            TowerBase currentTower = other.GetComponent<TowerBase>();
            currentTower.BuffRange(buffRange);
            currentTower.SetRateBoost(buffRate);
            currentTower.SetDamageBoost(buffDamage);
            towers.Add(other.transform);
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
                currentTower = tower.transform.GetComponent<TowerBase>();
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
                Debug.Log("oi");
                currentTower = tower.GetComponent<TowerBase>();
                currentTower.BuffRange(-buffRange);
                currentTower.SetRateBoost(-buffRate);
                currentTower.SetDamageBoost(-buffDamage);
            }
        }
    }

    public override void UpgradeTower()
    {
        GetComponent<SphereCollider>().radius = range;

        TowerBase currentTower;
        foreach (Transform tower in towers)
        {
            if (tower != null)
            {
                Debug.Log("oi");
                currentTower = tower.GetComponent<TowerBase>();
                currentTower.BuffRange(-buffRange);
                currentTower.SetRateBoost(-buffRate);
                currentTower.SetDamageBoost(-buffDamage);
            }
        }

        // upgrade logic

        foreach (Transform tower in towers)
        {
            if (tower != null)
            {
                Debug.Log("oi");
                currentTower = tower.GetComponent<TowerBase>();
                currentTower.BuffRange(buffRange);
                currentTower.SetRateBoost(buffRate);
                currentTower.SetDamageBoost(buffDamage);
            }
        }
    }
}
