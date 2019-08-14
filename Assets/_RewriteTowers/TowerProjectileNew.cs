using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerProjectileNew : TowerBase
{
    public float fireRate = 1f;
    private float initialFireRate;
    protected Transform target;
    protected EnemyBase targetEnemy;
    public int targettingStyle;

    public string enemyTag = "Enemy";
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

    private List<GameObject> possibleTargets;

    protected override void Awake()
    {
        base.Awake();
        initialFireRate = fireRate;
        bullets = new List<GameObject>();
        for (int i = 0; i < poolAmount; i++)
        {
            GameObject obj = (GameObject)Instantiate(bulletPrefab);
            obj.transform.SetParent(transform);
            obj.SetActive(false);
            bullets.Add(obj);
        }
    }

    // Start is called before the first frame update
    protected void Start()
    {
        possibleTargets = new List<GameObject>();
        GetComponent<SphereCollider>().radius = range;
        InvokeRepeating("UpdateTarget", 0f, .1f);
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


    // Update is called once per frame
    void Update()
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

    protected void UpdateTarget()
    {
        target = null;
        targetEnemy = null;
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

    void FindFirstTarget()
    {
        float shortestDist = Mathf.Infinity; //shortest distance to next waypoint
        int nextWaypoint = 0; //index of next waypoint
        List<GameObject> targetsToVerify = new List<GameObject>(possibleTargets);

        foreach (GameObject enemy in possibleTargets)
        {
            if (enemy.GetComponent<EnemyBase>().isDead)
            {
                targetsToVerify.Remove(enemy);
                //break;
            }
            else
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
        possibleTargets = new List<GameObject>(targetsToVerify);
    }

    void FindLastTarget()
    {
        float longestDist = 0; //shortest distance to next waypoint
        int lastWaypoint = 1000; //index of next waypoint
        List<GameObject> targetsToVerify = new List<GameObject>(possibleTargets);

        foreach (GameObject enemy in possibleTargets)
        {
            if (enemy == null) possibleTargets.Remove(enemy);
            else
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
        possibleTargets = new List<GameObject>(targetsToVerify);
    }

    //The target is the enemy with the highest HP
    private void FindStrongestTarget()
    {
        float highestHp = 0;
        List<GameObject> targetsToVerify = new List<GameObject>(possibleTargets);

        foreach (GameObject enemy in possibleTargets)
        {
            if (enemy == null) possibleTargets.Remove(enemy);
            else
            {
                if ((!seesInvisible && !enemy.GetComponent<EnemyBase>().GetInvisibleState()) || seesInvisible)
                {
                    float distToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
                    if (distToEnemy <= range && enemy.GetComponent<EnemyBase>().GetHp() > highestHp)
                    {
                        target = enemy.transform;
                        targetEnemy = enemy.GetComponent<EnemyBase>();
                        highestHp = enemy.GetComponent<EnemyBase>().GetHp();
                    }
                }
            }
        }
        possibleTargets = new List<GameObject>(targetsToVerify);
    }

    //The target is the enemy with the lowest HP
    private void FindWeakestTarget()
    {
        float lowestHp = Mathf.Infinity;
        List<GameObject> targetsToVerify = new List<GameObject>(possibleTargets);

        foreach (GameObject enemy in possibleTargets)
        {
            if (enemy == null) possibleTargets.Remove(enemy);
            else
            {
                if ((!seesInvisible && !enemy.GetComponent<EnemyBase>().GetInvisibleState()) || seesInvisible)
                {
                    float distToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
                    if (distToEnemy <= range && enemy.GetComponent<EnemyBase>().GetHp() < lowestHp)
                    {
                        target = enemy.transform;
                        targetEnemy = enemy.GetComponent<EnemyBase>();
                        lowestHp = enemy.GetComponent<EnemyBase>().GetHp();
                    }
                }
            }
        }
        possibleTargets = new List<GameObject>(targetsToVerify);
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.GetComponent<EnemyBase>())
        {
            EnemyBase enemy = col.GetComponent<EnemyBase>();
            if ((enemy.GetInvisibleState() && seesInvisible) || !enemy.invisible)
                possibleTargets.Add(enemy.gameObject);
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.GetComponent<EnemyBase>())
        {
            possibleTargets.Remove(col.gameObject);
        }
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
        //GameObject bulletGO = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        //ProjectileBase bullet = bulletGO.GetComponent<ProjectileBase>();
        //if (bullet != null)
        //{
        //    bullet.damage *= damageBoost;
        //    bullet.SetTarget(target);
        //}
        for (int i = 0; i < bullets.Count; i++)
        {
            if (!bullets[i].activeInHierarchy)
            {
                bullets[i].transform.position = firePoint.position;
                bullets[i].transform.rotation = firePoint.rotation;
                bullets[i].SetActive(true);
                bullets[i].GetComponent<ProjectileBase>().damage *= damageBoost;
                bullets[i].GetComponent<ProjectileBase>().SetTarget(target);
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
        GetComponent<SphereCollider>().radius = range;
        damageBoost += damageUpgrade;
        fireRate += fireRateUpgrade;
    }

    protected void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
