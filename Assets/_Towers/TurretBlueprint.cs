using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class TurretBlueprint
{
    public string name;
    // costs, color and sprite should probably go inside the turret itself to facilitate things
    public GameObject prefab;
    public int cost;

    //public GameObject upgradedPrefab;
    public int upgradeCost;

    public Color color;
    public Sprite sprite;
    
    //public int GetSellValue(bool upgraded)
    //{
    //    int value;
    //    if (upgraded) value = (cost + upgradeCost) / 2;
    //    else value = cost / 2;

    //    return value;
    //}

    public int GetSellValue(int numberOfUpgrades)
    {
        int value = cost/2;
        
        for (int i = 0; i < numberOfUpgrades; i++)
        {
            value = (cost + upgradeCost*i) / 2;
        }
        return value;
    }

    public int GetUpgradeCost()
    {
        return upgradeCost;
    }
}
