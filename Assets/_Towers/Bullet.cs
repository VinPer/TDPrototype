using UnityEngine;

public class Bullet : MonoBehaviour
{
    protected Transform target;

    public float speed = 70f;
    public float explosionRadius = 0f;
    public float damage = 10f;
    public float piercingValue = 0f;
    public GameObject impactEffect;

    public string mode = "homing";

    public void Seek(Transform _target)
    {
        if (mode == "homing") target = _target;
        else
        {
            target = new GameObject().transform;
            target.position = _target.position;
        }
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 dir = target.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        if(dir.magnitude <= distanceThisFrame)
        {
            HitTarget();
            return;
        }

        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
        transform.LookAt(target);
    }

    protected void HitTarget()
    {
        GameObject effectIns = (GameObject) Instantiate(impactEffect, transform.position, transform.rotation);
        Destroy(effectIns, 5f);
        
        if (explosionRadius > 0f)
        {
            Explode();
        } else
        {
            Damage(target);
        }

        Destroy(gameObject);
    }

    protected void Damage(Transform enemy)
    {
        Enemy e = enemy.GetComponent<Enemy>();
        e.TakeDamage(damage, piercingValue);
    }

    protected void Explode()
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

    protected void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
