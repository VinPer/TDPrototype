using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TurretBlueprint
{
    public GameObject prefab;
    public int cost;

    public GameObject upgradedPrefab;
    public int upgradeCost;

    public int GetSellValue(bool upgraded)
    {
        int value;
        if (upgraded) value = (cost + upgradeCost) / 2;
        else value = cost / 2;

        return value;
    }
}
