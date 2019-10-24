using UnityEngine;

public class ProjectileBase : MonoBehaviour
{
    public float damage;
    public float penetration;
    public Enums.Element debuffElement;
    public float debuffIntensity;
    public float debuffDuration;

    private Vector3 initialSize;


    public float speed = 50f;
    private float initialSpeed;
    public float acceleration = 0f;
    public bool seeking = false;
    private bool initialSeeking;
    [HideInInspector]
    public float explosionRadius;
    public int durability;
    protected Transform target;

    public GameObject impactEffect;

    [HideInInspector]
    public float decayTimer;
    protected Vector3 direction;

    protected virtual void Awake()
    {
        initialSpeed = speed;
        initialSize = transform.localScale;
        initialSeeking = seeking;
    }

    protected virtual void Destroy()
    {
        speed = initialSpeed;
        transform.localScale = initialSize;
        target = null;
        gameObject.SetActive(false);
        seeking = initialSeeking;
    }
    
    protected virtual void FixedUpdate()
    {
        if (decayTimer <= 0f || durability <= 0f)
        {
            Destroy();
            return;
        }
        
        if (seeking)
        {
            direction = target.position - transform.position;
            transform.LookAt(target);
            if (target.GetComponent<EnemyBase>().isDead)
                seeking = false;
        }

        float distanceThisFrame = speed * Time.deltaTime;

        transform.Translate(direction.normalized * distanceThisFrame, Space.World);
        decayTimer -= Time.deltaTime;
    }

    protected virtual void Hit(Transform hitPart)
    {
        //Assumes self destroying Impact Effect
        GameObject effectIns = Instantiate(impactEffect, hitPart.position, hitPart.rotation);
        //Destroy(effectIns, 1.5f);

        EnemyBase enemy = hitPart.GetComponent<EnemyBase>();
        if (enemy == null)
        {
            Destroy();
            return;
        } 
        //Debug.Log("Damage: " + damage);
        //Debug.Log("Durability: " + durability);
        durability--;

        if (explosionRadius > 0f)
        {
            Explode();
        }
        else
        {
            Damage(hitPart);
        }
        // include logic for checking durability
        if(durability <= 0 || seeking)
        {
            //Destroy(gameObject);
            Destroy();
        }
    }

    protected void Damage(Transform enemy)
    {
        EnemyBase e = enemy.GetComponent<EnemyBase>();

        //e could be null since now Hit() passes only the transform of where it hit 
        if (e != null){
            e.TakeDamage(damage, penetration, debuffElement);
        
            if (debuffElement != Enums.Element.none){
                e.ActivateDebuff(debuffIntensity, debuffDuration, debuffElement);
            }
        }
    }

    protected void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                Damage(collider.transform);
            }
        }
    }

    public float GetDamage()
    {
        return damage;
    }

    public float GetExplosionRadius()
    {
        return explosionRadius;
    }

    public void SetDamage(float value)
    {
        if (value < 0f) Debug.Log("Incorrect value to update damage!");
        else damage = value;
    }

    public int GetDurability()
    {
        return durability;
    }

    public Vector3 GetInitialSize()
    {
        return initialSize;
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

    public void SetExplosionRadius(float value)
    {
        if (value < 0f) Debug.Log("Incorrect value to update explosion radius!");
        else explosionRadius = value;
    }
    public Vector3 GetDirection()
    {
        return direction;
    }
    public void SetDirection(Vector3 dir)
    {
        direction = dir;
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
        if (!col.CompareTag("BlackHole") && !col.CompareTag("Range"))
        {
            Hit(col.transform);
        }
    }
}
