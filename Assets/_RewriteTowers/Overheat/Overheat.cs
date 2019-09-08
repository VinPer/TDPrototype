using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Overheat : TowerProjectile
{
    public Transform firePoints;
    private Transform[] fps = new Transform[6];

    protected override void Start()
    {
        targetStyles = null;
        for (int i = 0; i < fps.Length; i++)
        {
            fps[i] = firePoints.GetChild(i).GetComponent<Transform>();
        }
        base.Start();
    }
    protected override void Update()
    {
        if (target == null) return;
        if (fireCountdown <= 0f)
        {
            Shoot();
            fireCountdown = 1f / fireRate;
        }

        fireCountdown -= Time.deltaTime;
    }
    protected override void UpdateTarget()
    {
        target = null;
        targetEnemy = null;
        foreach (GameObject enemy in WaveSpawner.EnemiesAlive)
        {
            if (seesInvisible || (!seesInvisible && !enemy.GetComponent<EnemyBase>().GetInvisibleState()))
            {
                float distToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
                if (distToEnemy <= range)
                {
                target = enemy.transform;
                targetEnemy = enemy.GetComponent<EnemyBase>();
                }
            }
        }
    }
    protected override void Shoot()
    {
        foreach(Transform fp in fps)
        {
            for (int i = 0; i < bullets.Count; i++)
            {
                if (!bullets[i].activeInHierarchy)
                {
                    Transform t = bullets[i].transform;
                    ProjectileBase p = bullets[i].GetComponent<ProjectileBase>();
                    t.position = new Vector3(fp.position.x,target.position.y,fp.position.z);
                    t.rotation = fp.rotation;
                    bullets[i].SetActive(true);
                    p.damage *= damageBoost;
                    p.SetTarget(target);
                    Vector3 dir = fp.GetChild(0).transform.position - t.position;
                    dir.y = 0;
                    p.SetDirection(dir);
                    //Debug.Log(p.GetDirection());
                    break;
                }
            }
        }
        if (GetComponent<AudioSource>())
            GetComponent<AudioSource>().Play();

    }
}
