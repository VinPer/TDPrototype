using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puddle : MonoBehaviour
{
    public float debuffIntensity = 10f;
    public float debuffDuration = 2f;
    public Enums.Element debuffType;
    public float duration = 3f;
    private void Update()
    {
        duration -= Time.deltaTime;

        if (duration <= 0f)
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerStay(Collider col)
    {
        if (col.tag != "Enemy") return;
        EnemyBase e = col.GetComponent<EnemyBase>();
        if(e != null)
        e.ActivateDebuff(debuffIntensity,debuffDuration,debuffType);
    }
}
