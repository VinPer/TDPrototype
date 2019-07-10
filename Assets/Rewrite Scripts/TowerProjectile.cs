﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerProjectile : TowerBase
{
    //private Magazine magazine;
    public float fireRate = 1f;
    public Transform target;
    public Enemy targetEnemy;
    public int targettingStyle;

    public string enemyTag = "Enemy";
    public Transform partToRotate;
    public float turnSpeed = 10f;
    public Transform firePoint;
    protected float fireCountdown = 0f;
    public GameObject bulletPrefab;

    protected virtual void Start()
    {
        InvokeRepeating("GetTarget", 0f, 0.5f);
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
        Bullet bullet = bulletGO.GetComponent<Bullet>();
        //bullet.damage *= damageBoost;

        if (bullet != null)
        {
            bullet.Seek(target);
        }
    }

    public void UpdateTarget(Transform newTarget)
    {
        target = newTarget;
        targetEnemy = newTarget.GetComponent<Enemy>();
    }

    public void UpdateFireRate(float value)
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
