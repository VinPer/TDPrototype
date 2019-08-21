using System.Collections.Generic;
using UnityEngine;

public class TowerLaser : TowerNonProjectile
{
    public LineRenderer lineRenderer;
    public ParticleSystem impactEffect;
    public Light impactLight;

    public int targettingStyle;
    private Transform target;
    private EnemyBase targetEnemy;

    public Transform firePoint;
    public Transform partToRotate;
    public float turnSpeed = 10f;
    private float fireCountdown = 0;
    public string enemyTag = "Enemy";

    public enum PossibleTargets { first, last, strongest, weakest };
    public PossibleTargets possibleTargets = PossibleTargets.first; //First by default

    protected void Start()
    {
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

    private void Update()
    {
        if (target == null)
        {
            GetComponent<AudioSource>().Stop();
            if (lineRenderer.enabled)
            {
                lineRenderer.enabled = false;
                impactEffect.Stop();
                impactLight.enabled = false;
            }
            return;
        }

        LockOnTarget();

        if (fireCountdown <= 0f)
        {
            Laser();
            fireCountdown = 1f / triggerRate;
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

    private void LockOnTarget()
    {
        // Target lock on for nearest target
        Vector3 dir = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
        partToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);
    }

    private void Laser()
    {
        targetEnemy.TakeDamage(damage, penetration, debuffElement);
        GetComponent<AudioSource>().Play();
        if (!lineRenderer.enabled)
        {
            lineRenderer.enabled = true;
            impactEffect.Play();
            impactLight.enabled = true;
        }

        lineRenderer.SetPosition(0, firePoint.position);
        lineRenderer.SetPosition(1, target.position);

        Vector3 dir = firePoint.position - target.position;

        impactEffect.transform.rotation = Quaternion.LookRotation(dir);
        impactEffect.transform.position = target.position + dir.normalized * .5f;
    }

    protected override void UpgradeStatus()
    {
        range += rangeUpgrade;
    }

}
