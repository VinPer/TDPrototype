using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TurretBlueprint
{
    // costs, color and sprite should probably go inside the turret itself to facilitate things
    public GameObject prefab;
    public int cost;

    public GameObject upgradedPrefab;
    public int upgradeCost;

    public Color color;
    public Sprite sprite;

    public string name;
    public int GetSellValue(bool upgraded)
    {
        int value;
        if (upgraded) value = (cost + upgradeCost) / 2;
        else value = cost / 2;

        return value;
    }
}
