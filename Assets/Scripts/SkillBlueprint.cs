using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SkillBlueprint
{
    public GameObject prefab;
    public float ySpawn;
    public float initialCooldown;
    public float currentCooldown;
    public string name;
}
