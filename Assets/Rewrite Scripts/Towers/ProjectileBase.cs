﻿using UnityEngine;

public class ProjectileBase : MonoBehaviour
{
    public float damage = 10f;
    public float penetration = 0f;
    public string debuffElement = "none";
    public float debuffIntensity = 0f;
    public float debuffDuration = 0f;

    public float speed = 50f;
    public float acceleration = 0f;
    public bool seeking = false;
    public float explosionRadius = 0f;
    public float durability = 100f;
    private Transform target;

    public GameObject impactEffect;

    public float initialDecayTimer;
    public float decayTimer = 2f;
    public Vector3 direction;

    private void Start()
    {
        initialDecayTimer = decayTimer;
    }

    private void Update()
    {

        if ((target == null && seeking) || decayTimer <= 0f || durability <= 0f)
        {
            Destroy(gameObject);
            return;
        }

        if (seeking)
        {
            direction = target.position - transform.position;
            transform.LookAt(target);
        }

        float distanceThisFrame = speed * Time.deltaTime;

        //if (direction.magnitude <= distanceThisFrame)
        //{
        //    HitTarget();
        //    return;
        //}

        transform.Translate(direction.normalized * distanceThisFrame, Space.World);
        decayTimer -= Time.deltaTime;
    }

    private void HitTarget()
    {
        GameObject effectIns = (GameObject)Instantiate(impactEffect, transform.position, transform.rotation);
        Destroy(effectIns, 5f);

        if (explosionRadius > 0f)
        {
            Explode();
        }
        else
        {
            Damage(target);
            // include logic for reducing durability
        }
        
        // include logic for checking durability
        Destroy(gameObject);
    }

    private void Damage(Transform enemy)
    {
        Enemy e = enemy.GetComponent<Enemy>();
        e.TakeDamage(damage, penetration);
    }

    private void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider collider in colliders)
        {
            if (collider.tag == "Enemy")
            {
                Damage(collider.transform);
            }
        }
    }

    public void SetSpeed(float value)
    {
        if (value <= 0f) Debug.Log("Incorrect value to update speed!");
        else speed = value;
    }

    public void SetAcceleration(float value)
    {
        if (value <= 0f) Debug.Log("Incorrect value to update acceleration!");
        else acceleration = value;
    }

    public void SetTarget(Transform newTarget)
    {
        if (seeking) target = newTarget;
        else
        {
            direction = newTarget.position - transform.position;
            transform.LookAt(newTarget);
        }
    }

    public void SetTarget(Vector3 dir)
    {
        direction = dir;
    }

    public void UpdateDurability(float value)
    {
        if (value <= 0f) Debug.Log("Incorrect value to update durability!");
        else durability -= value;
    }

    public void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Enemy")
        {
            target = col.transform;
            HitTarget();
        }
    }
}