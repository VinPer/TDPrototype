using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerLaser : TowerNonProjectile
{
    public LineRenderer lineRenderer;
    public ParticleSystem impactEffect;
    public Light impactLight;

    public int targettingStyle;
    private Transform target;
    private Enemy targetEnemy;

    public Transform firePoint;
    public Transform partToRotate;
    public float turnSpeed = 10f;
    private float fireCountdown = 0;
    public string enemyTag = "Enemy";

    private void Start()
    {
        InvokeRepeating("GetTarget", 0f, 0.5f);
    }

    private void Update()
    {
        if (target == null)
        {
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

    public void GetTarget()
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
            targetEnemy = target.GetComponent<Enemy>();
        }
        else
        {
            target = null;
            targetEnemy = null;
        }
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
        targetEnemy.TakeDamage(damage, penetration);

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

    public void UpdateTarget(Transform newTarget)
    {
        target = newTarget;
        targetEnemy = newTarget.GetComponent<Enemy>();
    }

    public override void UpgradeTower()
    {
        // upgrade logic
    }
}
