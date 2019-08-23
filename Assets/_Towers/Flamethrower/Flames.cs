using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flames : MonoBehaviour
{
    public float damage = 1f;
    [Range(0f,1f)]
    public float piercingValue = 0f;
    public float damageRate = 100f;
    private float damageCountdown = 0f;
    public float duration = 5f;
    public GameObject fireEffect;
    private GameObject flameEffect;
    private Enums.Element fire = Enums.Element.fire;
    public string enemyTag = "Enemy";

    // Start is called before the first frame update
    void Start()
    {
        Vector3 pos;
        for (int i = 0; i < 9; i ++)
        {
            pos = new Vector3(transform.position.x, transform.position.y, transform.position.z + i);
            flameEffect = Instantiate(fireEffect, transform);
            flameEffect.transform.position = pos;
        }
    }

    // Damages everything within its area every 1f / damageRate seconds
    void Update()
    {
        //if (damageCountdown <= 0f)
        //{
        //    BurnArea();
        //    damageCountdown = 1f / damageRate;
        //}

        //damageCountdown -= Time.deltaTime;

    }

    // This requires optimization to the collision area
    void BurnArea()
    {
        Vector3 pos = new Vector3(transform.position.x, transform.position.y, transform.position.z + 5.5f);
        Collider[] colliders = Physics.OverlapBox(pos, transform.localScale / 2, transform.rotation);
        foreach (Collider collider in colliders)
        {
            if (collider.tag == "Enemy")
            {
                BurnTarget(collider.transform);
            }
        }
    }

    private void OnTriggerStay(Collider col)
    {
        if (col.tag != "Enemy") return;
        BurnTarget(col.transform);
    }

    private void BurnTarget(Transform enemy)
    {
        EnemyBase e = enemy.GetComponent<EnemyBase>();
        e.TakeDamage(damage * Time.deltaTime, piercingValue,fire);
        e.ActivateDebuff(damage, duration, fire);
    }
}
