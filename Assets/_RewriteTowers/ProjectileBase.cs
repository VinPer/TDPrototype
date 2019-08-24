using UnityEngine;

public class ProjectileBase : MonoBehaviour
{
    public float damage = 10f;
    private float initialDamage;
    [Range(0f,1f)]
    public float penetration = 0f;
    [HideInInspector]
    public Enums.Element debuffElement;
    public float debuffIntensity = 0f;
    public float debuffDuration = 0f;

    public float speed = 50f;
    public float acceleration = 0f;
    public bool seeking = false;
    public float explosionRadius = 0f;
    public int durability = 1;
    private int initialDurability;
    protected Transform target;

    public GameObject impactEffect;

    private float initialDecayTimer;
    public float decayTimer = 2f;
    protected Vector3 direction;

    protected void Awake()
    {
        initialDecayTimer = decayTimer;
        initialDurability = durability;
        initialDamage = damage;
    }
    
    protected void Destroy()
    {
        decayTimer = initialDecayTimer;
        durability = initialDurability;
        damage = initialDamage;

        target = null;
        gameObject.SetActive(false);
    }
    
    protected virtual void Update()
    {
        if ((!target.gameObject.active && seeking) || decayTimer <= 0f || durability <= 0f)
        {
            target = null;
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
        Destroy(effectIns, 1.5f);

        //Debug.Log("Damage: " + damage);
        //Debug.Log("Durability: " + durability);

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
        if (debuffElement != Enums.Element.none)
            e.ActivateDebuff(debuffIntensity, debuffDuration, debuffElement);
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
