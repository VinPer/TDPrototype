using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargerOrb : Bullet
{
    private Charger turret;
    private Transform firePoint;
    private bool released = false;

    public float range = 10f;
    public string enemyTag = "Enemy";

    [Header("Debuff Playground")]
    public float debuffMultiplier = 10f;
    public float debuffDuration = 10f;
    public string debuffName = "fire";

    private void Start()
    {
        turret = transform.parent.GetComponentInParent<Charger>();
        firePoint = transform.parent;
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (target == null || released) return;
        else
        {
            released = true;
            transform.SetParent(null);

            StartCoroutine(Seek());
        }
    }

    private void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;
        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }

        if (nearestEnemy != null && shortestDistance <= range)
        {
            target = nearestEnemy.transform;
        }
        else
        {
            target = null;
        }
    }

    IEnumerator Seek()
    {
        while (released)
        {
            if (target == null)
            {
                StartCoroutine(ReturnToTower());
                yield break;
            }

            Vector3 dir = target.position - transform.position;
            float distanceThisFrame = speed * Time.deltaTime;

            if (dir.magnitude <= distanceThisFrame)
            {
                turret.currentOrbAmount--;
                target.GetComponent<Enemy>().ActivateDebuff(debuffMultiplier, debuffDuration, debuffName);
                HitTarget();
                yield break;
            }

            transform.Translate(dir.normalized * distanceThisFrame, Space.World);
            transform.LookAt(target);
            
            yield return null;
        }
    }

    IEnumerator ReturnToTower()
    {
        bool done = false;
        while (!done)
        {
            Vector3 dir = firePoint.position - transform.position + turret.offset;
            float distanceThisFrame = speed * Time.deltaTime;

            if (dir.magnitude <= distanceThisFrame)
            {
                transform.SetParent(firePoint);
                done = true;
                released = false;
                yield break;
            }

            transform.Translate(dir.normalized * distanceThisFrame, Space.World);
            transform.LookAt(firePoint);

            yield return null;
        }
    }
}
