using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkillGlobal : SkillBase
{
    public string targetTag = "Enemy";
    public GameObject[] enemies;

    protected void FindTargets()
    {
        enemies = WaveSpawner.EnemiesAlive.ToArray();
    }
}
