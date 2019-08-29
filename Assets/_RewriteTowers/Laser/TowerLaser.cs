using System.Collections.Generic;
using UnityEngine;

public class TowerLaser : TowerNonProjectile
{
    private float multiplicator = .1f;

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

    //public enum TargetStyle { first, last, strongest, weakest };
    //public TargetStyle targetStyle = TargetStyle.first; //First by default
    public List<string> targetStyles = new List<string> { "First", "Last", "Strongest", "Weakest","bola" };
    public string targetSelected;

    protected void Start()
    {
        InvokeRepeating("UpdateTarget", 0f, 0.2f);
        targetSelected = "First";
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
        //switch (targetStyle)
        //{
        //    case (TargetStyle.first):
        //        FindFirstTarget();
        //        break;
        //    case (TargetStyle.last):
        //        FindLastTarget();
        //        break;
        //    case (TargetStyle.strongest):
        //        FindStrongestTarget();
        //        break;
        //    case (TargetStyle.weakest):
        //        FindWeakestTarget();
        //        break;
        //}
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

    private void Laser()
    {
        targetEnemy.TakeDamage(damage * multiplicator * Time.deltaTime, penetration, element);
        if (multiplicator < 1) multiplicator *= 1.01f;
        else multiplicator = 1;

        //Sound
        if (!GetComponent<AudioSource>().isPlaying)
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
