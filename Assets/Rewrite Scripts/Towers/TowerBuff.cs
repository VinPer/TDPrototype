using System.Collections.Generic;
using UnityEngine;

public class TowerBuff : TowerBase
{
    public string towerTag = "Tower";

    public float rangeBuff = 10f;
    public float rateBuff = 10f;
    public float damageBuff = 10f;

    private List<Transform> towers;

    private void Start()
    {
        rangeBuff = rangeBuff / 100 + 1;
        rateBuff = rateBuff / 100 + 1;
        damageBuff = damageBuff / 100 + 1;

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
                currentTower.SetRangeBoost(rangeBuff);
                currentTower.SetRateBoost(rateBuff);
                currentTower.SetDamageBoost(damageBuff);
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
                currentTower.SetRangeBoost(rangeBuff);
                currentTower.SetRateBoost(rateBuff);
                currentTower.SetDamageBoost(damageBuff);
            }
        }
    }

    public override void UpgradeTower()
    {
        // upgrade logic
    }
}
