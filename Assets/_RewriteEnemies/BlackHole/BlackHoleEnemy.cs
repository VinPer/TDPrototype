using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHoleEnemy : EnemyBase
{
    [Header("Black Hole Range Game Object")]
    public GameObject blackHole;
    [Header("Black Hole Effect Range")]
    public float blackHoleRange = 3f;

    private SphereCollider blackHoleCollider;

    protected override void Start(){
        base.Start();

        blackHoleCollider = blackHole.GetComponent<SphereCollider>();
        blackHoleCollider.radius = blackHoleRange;
    }
}
