using System.Collections.Generic;
using UnityEngine;

public class TowerFreezer : TowerNonProjectile
{
    private float multiplicator = .1f;
    public float reloadTime = 3f;

    public int targettingStyle;
    private Transform target;
    private EnemyBase targetEnemy;

    public Transform partToRotate;
    public float turnSpeed = 10f;
    private float fireCountdown = 0;
    public string enemyTag = "Enemy";

    //public enum TargetStyle { first, last, strongest, weakest };
    //public TargetStyle targetStyle = TargetStyle.first; //First by default
    public List<string> targetStyles = new List<string> { "First", "Last", "Strongest", "Weakest" };
    public string targetSelected;

    protected void Start()
    {
        targetSelected = "First";
        InvokeRepeating("UpdateTarget", 0f, 0.2f);
    }

    private void Update()
    {
        if (target == null)
        {
            return;
        }

        LockOnTarget();
    }

    protected virtual void UpdateTarget()
    {
        switch (targetSelected)
        {
            case ("First"):
                FindFirstTarget();
                break;
            case ("Last"):
                FindLastTarget();
                break;
            case ("Strongest"):
                FindStrongestTarget();
                break;
            case ("Weakest"):
                FindWeakestTarget();
                break;
        }
        if (target == null) multiplicator = .1f;
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

    private void LockOnTarget()
    {
        // Target lock on for nearest target
        Vector3 dir = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
        partToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);
    }

    protected override void UpgradeStatus()
    {
        string _damage = "damage";
        if (upgrades[_damage] < TowerUpgrade.instance.towers[gameObject.name][_damage])
        {
            damage += damageBoost;
            upgrades[_damage]++;
        }

        string _chargeTime = "chargeTime";
        if (upgrades[_chargeTime] < TowerUpgrade.instance.towers[gameObject.name][_chargeTime])
        {
            reloadTime -= .5f;
            upgrades[_chargeTime]++;
        }
    }

}
