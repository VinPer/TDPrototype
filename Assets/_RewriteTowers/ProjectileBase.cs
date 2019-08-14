using UnityEngine;

public class ProjectileBase : MonoBehaviour
{
    public float damage = 10f;
    public float penetration = 0f;
    public Enums.Element debuffElement;
    public float debuffIntensity = 0f;
    public float debuffDuration = 0f;

    public float speed = 50f;
    public float acceleration = 0f;
    public bool seeking = false;
    public float explosionRadius = 0f;
    public int durability = 1;
    private int initialDurability;
    private Transform target;

    public GameObject impactEffect;

    public float initialDecayTimer;
    public float decayTimer = 2f;
    public Vector3 direction;

    private void Start()
    {
        initialDecayTimer = decayTimer;
        initialDurability = durability;
    }
    
    void Destroy()
    {
        decayTimer = initialDecayTimer;
        durability = initialDurability;
        target = null;
        seeking = false;
        gameObject.SetActive(false);
    }
    
    private void Update()
    {
        if ((target == null && seeking) || decayTimer <= 0f || durability <= 0f)
        {
            //Destroy(gameObject);
            Destroy();
            return;
        }

        if (seeking)
        {
            direction = target.position - transform.position;
            transform.LookAt(target);
        }

        float distanceThisFrame = speed * Time.deltaTime;

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
            durability--;
            // include logic for reducing durability
        }
        
        // include logic for checking durability
        if(durability <= 0 || seeking)
        {
            //Destroy(gameObject);
            Destroy();
        }
    }

    private void Damage(Transform enemy)
    {
        EnemyBase e = enemy.GetComponent<EnemyBase>();
        e.TakeDamage(damage, penetration, debuffElement);
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

    public float GetDamage()
    {
        return damage;
    }

    public void SetDamage(float value)
    {
        if (value <= 0f) Debug.Log("Incorrect value to update damage!");
        else damage = value;
    }

    public int GetDurability()
    {
        return durability;
    }

    public void SetDurability(int value)
    {
        if (value <= 0) Debug.Log("Incorrect value to update durability!");
        else durability = value;
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

    public void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Enemy")
        {
            target = col.transform;
            HitTarget();
        }
    }
}
