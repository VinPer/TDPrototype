using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffTower : MonoBehaviour
{
    public float buffRange = 5f;
    public string towerTag = "Tower";

    public float rangeBoost = 10f;
    public float fireRateBoost = 10f;
    public float damageBoost = 10f;

    private List<Transform> towers;

    private void Start()
    {
        rangeBoost = rangeBoost / 100 + 1;
        fireRateBoost = fireRateBoost / 100 + 1;
        damageBoost = damageBoost / 100 + 1;

        towers = new List<Transform>();
        InvokeRepeating("FindTowers", 0f, 2f);
    }

    private void FindTowers()
    {
        GameObject[] turrets = GameObject.FindGameObjectsWithTag(towerTag);
        float distanceToTower;
        Turret currentTower;
        foreach (GameObject tower in turrets)
        {
            distanceToTower = Vector3.Distance(transform.position, tower.transform.position);
            if (distanceToTower <= buffRange && !towers.Contains(tower.transform))
            {
                towers.Add(tower.transform);
                currentTower = tower.transform.GetComponent<Turret>();
                currentTower.range *= rangeBoost;
                currentTower.fireRate *= fireRateBoost;
                currentTower.damageBoost *= damageBoost;
            }
        }
    }

    private void OnDestroy()
    {
        Turret currentTower;
        foreach (Transform tower in towers)
        {
            if (tower != null)
            {
                currentTower = tower.GetComponent<Turret>();
                currentTower.range /= rangeBoost;
                currentTower.fireRate /= fireRateBoost;
                currentTower.damageBoost /= damageBoost;
            }
        }
    }
}
