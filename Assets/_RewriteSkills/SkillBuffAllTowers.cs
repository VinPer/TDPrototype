using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBuffAllTowers : SkillGlobal
{
    public float buffRange = 10f;
    public float buffRate = 10f;
    public float buffDamage = 10f;
    public float duration = 10f;

    private GameObject[] towers;
    private string towerTag = "Tower";

    private void Start()
    {
        FindTowers();
        BuffTowers();
    }

    private void Update()
    {
        duration -= Time.deltaTime;

        if (duration <= 0f)
        {
            FindTowers();
            End();
        }
    }

    private void FindTowers()
    {
        towers = GameObject.FindGameObjectsWithTag(towerTag);
    }

    private void BuffTowers()
    {
        TowerBase tower;
        foreach(GameObject target in towers)
        {
            tower = target.GetComponent<TowerBase>();
            tower.BuffRange(buffRange);
            tower.SetRateBoost(buffRate+1f);
            tower.SetDamageBoost(buffDamage+1f);
        }
    }
    
    private void End()
    {
        TowerBase tower;
        foreach (GameObject target in towers)
        {
            tower = target.GetComponent<TowerBase>();
            tower.BuffRange(-buffRange);
            tower.SetRateBoost(1f);
            tower.SetDamageBoost(1f);
        }

        Destroy(gameObject);
    }

    //private void OnTriggerEnter(Collider col)
    //{
    //    TowerBase tower = col.GetComponent<TowerBase>();
    //    tower.BuffRange(buffRange);
    //    tower.SetRateBoost(buffRate);
    //    tower.SetDamageBoost(buffDamage);
    //}
}
