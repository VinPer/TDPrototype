using System.Collections.Generic;
using UnityEngine;

public class TowerProjectile : TowerBase
{
    //private Magazine magazine;
    public float fireRate = 1f;
    private float initialFireRate;
    protected Transform target;
    protected EnemyBase targetEnemy;
    
    public Transform partToRotate;
    public float turnSpeed = 10f;
    public Transform firePoint;
    protected float fireCountdown = 0f;

    public int poolAmount = 3;
    private List<GameObject> bullets;
    public GameObject bulletPrefab;

    public float damageUpgrade = .1f;
    public float fireRateUpgrade = .1f;

    public enum TargetStyle { first, last, strongest, weakest };
    public TargetStyle targetStyle = TargetStyle.first; //First by default

    protected override void Awake()
    {
        base.Awake();
        initialFireRate = fireRate;
        SpawnBulletPool();
    }

    protected virtual void SpawnBulletPool()
    {
        bullets = new List<GameObject>();
        for (int i = 0; i < poolAmount; i++)
        {
            GameObject obj = (GameObject)Instantiate(bulletPrefab);
            obj.transform.SetParent(transform);
            obj.GetComponent<ProjectileBase>().debuffElement = element;
            obj.SetActive(false);
            bullets.Add(obj);
        }
    }

    protected virtual void Start()
    {
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

    protected virtual void Update()
    {
        if (target == null) return;

        LockOnTarget();

        if (fireCountdown <= 0f)
        {
            Shoot();
            fireCountdown = 1f / fireRate;
        }

        fireCountdown -= Time.deltaTime;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        fireRate = initialFireRate;
        fireCountdown = 0;
        target = null;
        targetEnemy = null;
        targetStyle = TargetStyle.first;
    }

    protected virtual void UpdateTarget()
    {
        switch (targetStyle)
        {
            case (TargetStyle.first):
                FindFirstTarget();
                break;
            case (TargetStyle.last):
                FindLastTarget();
                break;
            case (TargetStyle.strongest):
                FindStrongestTarget();
                break;
            case (TargetStyle.weakest):
                FindWeakestTarget();
                break;
        }
    }

    //The target is the first enemy
    private void FindFirstTarget()
    {
        target = null;
        targetEnemy = null;

        float shortestDist = Mathf.Infinity; //shortest distance to next waypoint
        int nextWaypoint = 0; //index of next waypoint

        foreach (GameObject enemy in WaveSpawner.EnemiesAlive)
        {
            if (seesInvisible || (!seesInvisible && !enemy.GetComponent<EnemyBase>().GetInvisibleState()))
            {
                float distToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
                if (distToEnemy <= range)
                {
                    if (enemy.GetComponent<EnemyMovement>().GetWaypointIndex() >= nextWaypoint)
                    {
                        nextWaypoint = enemy.GetComponent<EnemyMovement>().GetWaypointIndex();
                        if (enemy.GetComponent<EnemyMovement>().distToNextWaypoint < shortestDist)
                        {
                            target = enemy.transform;
                            targetEnemy = enemy.GetComponent<EnemyBase>();
                            shortestDist = enemy.GetComponent<EnemyMovement>().distToNextWaypoint;
                        }
                    }
                }
            }
        }
    }

    private void FindLastTarget()
    {
        target = null;
        targetEnemy = null;

        float longestDist = 0; //shortest distance to next waypoint
        int lastWaypoint = 1000; //index of next waypoint

        foreach (GameObject enemy in WaveSpawner.EnemiesAlive)
        {
            if (seesInvisible || (!seesInvisible && !enemy.GetComponent<EnemyBase>().GetInvisibleState()))
            {
                float distToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
                if (distToEnemy <= range)
                {
                    if (enemy.GetComponent<EnemyMovement>().GetWaypointIndex() <= lastWaypoint)
                    {
                        lastWaypoint = enemy.GetComponent<EnemyMovement>().GetWaypointIndex();
                        if (enemy.GetComponent<EnemyMovement>().distToNextWaypoint > longestDist)
                        {
                            target = enemy.transform;
                            targetEnemy = enemy.GetComponent<EnemyBase>();
                            longestDist = enemy.GetComponent<EnemyMovement>().distToNextWaypoint;
                        }
                    }
                }
            }
        }
    }

    //The target is the enemy with the highest HP
    private void FindStrongestTarget()
    {
        target = null;
        targetEnemy = null;

        float highestHp = 0;

        foreach (GameObject enemy in WaveSpawner.EnemiesAlive)
        {
            if (seesInvisible || (!seesInvisible && !enemy.GetComponent<EnemyBase>().GetInvisibleState()))
            {
                float distToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
                if (distToEnemy <= range)
                {
                    if (highestHp < enemy.GetComponent<EnemyBase>().GetHp())
                    {
                        target = enemy.transform;
                        targetEnemy = enemy.GetComponent<EnemyBase>();
                        highestHp = enemy.GetComponent<EnemyBase>().GetHp();
                    }
                }
            }
        }
    }
    //The target is the enemy with the lowest HP
    private void FindWeakestTarget()
    {
        target = null;
        targetEnemy = null;

        float lowestHp = Mathf.Infinity;

        foreach (GameObject enemy in WaveSpawner.EnemiesAlive)
        {
            if (seesInvisible || (!seesInvisible && !enemy.GetComponent<EnemyBase>().GetInvisibleState()))
            {
                float distToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
                if (distToEnemy <= range)
                {
                    if (lowestHp > enemy.GetComponent<EnemyBase>().GetHp())
                    {
                        target = enemy.transform;
                        targetEnemy = enemy.GetComponent<EnemyBase>();
                        lowestHp = enemy.GetComponent<EnemyBase>().GetHp();
                    }
                }
            }
        }
    }

    public Transform GetTarget()
    {
        return target;
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
        targetEnemy = newTarget.GetComponent<EnemyBase>();
    }

    protected virtual void LockOnTarget()
    {
        // Target lock on for nearest target
        Vector3 dir = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
        partToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);
    }

    protected virtual void Shoot()
    {
        for (int i = 0; i < bullets.Count; i++)
        {
            if (!bullets[i].activeInHierarchy)
            {
                bullets[i].transform.position = firePoint.position;
                bullets[i].transform.rotation = firePoint.rotation;
                bullets[i].SetActive(true);
                bullets[i].GetComponent<ProjectileBase>().damage *= damageBoost;
                bullets[i].GetComponent<ProjectileBase>().SetTarget(target);
                if (GetComponent<AudioSource>())
                    GetComponent<AudioSource>().Play();
                break;
            }
        }
    }

    public void SetFireRate(float value)
    {
        if (value <= 0f) Debug.Log("Incorrect value to update fire rate!");
        else fireRate = value;
    }

    protected override void UpgradeStatus()
    {
        range += rangeUpgrade;
        damageBoost += damageUpgrade;
        fireRate += fireRateUpgrade;
    }

    protected void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }

}
