using System.Collections.Generic;
using UnityEngine;

public class TowerBuff : TowerBase
{
    public string towerTag = "Tower";

    public float buffRange = 10f;
    public float buffRate = 10f;
    public float buffDamage = 10f;

    private List<Transform> towers;

    private void Start()
    {
        buffRange = buffRange / 100 + 1;
        buffRate = buffRate / 100 + 1;
        buffDamage = buffDamage / 100 + 1;

        towers = new List<Transform>();
        FindTowers();
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
                currentTower.SetRangeBoost(buffRange);
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
                currentTower = tower.GetComponent<TowerBase>();
                currentTower.SetRangeBoost(1f);
                currentTower.SetRateBoost(1f);
                currentTower.SetDamageBoost(1f);
            }
        }
    }

    public override void UpgradeTower()
    {
        // upgrade logic
    }
}
