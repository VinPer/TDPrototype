using System.Collections.Generic;
using UnityEngine;

public class TowerProjectile : TowerBase
{
    //private Magazine magazine;
    public float fireRate = 1f;
    protected Transform target;
    protected EnemyBase targetEnemy;
    public int targettingStyle;

    public string enemyTag = "Enemy";
    public Transform partToRotate;
    public float turnSpeed = 10f;
    public Transform firePoint;
    protected float fireCountdown = 0f;
    public GameObject bulletPrefab;

    public enum PossibleTargets { first, last, strongest, weakest };
    public PossibleTargets possibleTargets = PossibleTargets.first; //First by default

    protected override void Start()
    {
        base.Start();
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

    protected virtual void UpdateTarget()
    {
        //Array of all th enemies
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag); //enemyTag is in TurretBase
        
        switch (possibleTargets)
        {
            case (PossibleTargets.first):
                FindFirstTarget(enemies);
                break;
            case (PossibleTargets.last):
                FindLastTarget(enemies);
                break;
            case (PossibleTargets.strongest):
                FindStrongestTarget(enemies);
                break;
            case (PossibleTargets.weakest):
                FindWeakestTarget(enemies);
                break;
        }
    }
    
    //The target is the first enemy
    private void FindFirstTarget(GameObject[] _enemies)
    {
        target = null;
        targetEnemy = null;

        List<GameObject> possibleTargets = new List<GameObject>();
        float shortestDist = Mathf.Infinity; //shortest distance to next waypoint
        int nextWaypoint = 0; //index of next waypoint

        foreach (GameObject enemy in _enemies)
        {
            if ((!seesInvisible && !enemy.GetComponent<EnemyBase>().GetInvisibleState()) || seesInvisible)
            {
                float distToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
                if (distToEnemy <= range && enemy.GetComponent<EnemyMovement>().GetWaypointIndex() >= nextWaypoint)
                {
                    if (enemy.GetComponent<EnemyMovement>().GetWaypointIndex() > nextWaypoint)
                    {
                        nextWaypoint = enemy.GetComponent<EnemyMovement>().GetWaypointIndex();
                        possibleTargets.Clear();
                        possibleTargets.Add(enemy);
                    }
                    else
                    {
                        possibleTargets.Add(enemy);
                    }
                }
            }
        }
        //check possible targets
        foreach (GameObject possibleTarget in possibleTargets)
        {
            if (possibleTarget.GetComponent<EnemyMovement>().distToNextWaypoint < shortestDist)
            {
                target = possibleTarget.transform;
                targetEnemy = possibleTarget.GetComponent<EnemyBase>();
                shortestDist = possibleTarget.GetComponent<EnemyMovement>().distToNextWaypoint;
            }
        }
    }

    private void FindLastTarget(GameObject[] _enemies)
    {
        target = null;
        targetEnemy = null;

        List<GameObject> possibleTargets = new List<GameObject>();
        float longestDist = 0; //shortest distance to next waypoint
        int lastWaypoint = 1000; //index of the last waypoint

        foreach (GameObject enemy in _enemies)
        {
            if ((!seesInvisible && !enemy.GetComponent<EnemyBase>().GetInvisibleState() || seesInvisible))
            {
                float distToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
                if (distToEnemy <= range && enemy.GetComponent<EnemyMovement>().GetWaypointIndex() <= lastWaypoint)
                {
                    if (enemy.GetComponent<EnemyMovement>().GetWaypointIndex() < lastWaypoint)
                    {
                        lastWaypoint = enemy.GetComponent<EnemyMovement>().GetWaypointIndex();
                        possibleTargets.Clear();
                        possibleTargets.Add(enemy);
                    }
                    else
                    {
                        possibleTargets.Add(enemy);
                    }
                }
            }
        }
        //check possible targets
        foreach (GameObject possibleTarget in possibleTargets)
        {
            if (possibleTarget.GetComponent<EnemyMovement>().distToNextWaypoint > longestDist)
            {
                target = possibleTarget.transform;
                targetEnemy = possibleTarget.GetComponent<EnemyBase>();
                longestDist = possibleTarget.GetComponent<EnemyMovement>().distToNextWaypoint;
            }
        }
    }

    //The target is the enemy with the highest HP
    private void FindStrongestTarget(GameObject[] _enemies)
    {
        target = null;
        targetEnemy = null;

        float highestHp = 0;

        foreach (GameObject enemy in _enemies)
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
    //The target is the enemy with the lowest HP
    private void FindWeakestTarget(GameObject[] _enemies)
    {
        target = null;
        targetEnemy = null;

        float lowestHp = Mathf.Infinity;

        foreach (GameObject enemy in _enemies)
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
        GameObject bulletGO = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        ProjectileBase bullet = bulletGO.GetComponent<ProjectileBase>();

        if (bullet != null)
        {
            //bullet.damage *= damageBoost;
            bullet.SetTarget(target);
        }
    }

    public void SetFireRate(float value)
    {
        if (value <= 0f) Debug.Log("Incorrect value to update fire rate!");
        else fireRate = value;
    }

    public override void UpgradeTower()
    {
        // upgrade logic
    }

    protected void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
