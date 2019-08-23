using UnityEngine;

public class SkillBomb : SkillTargetted
{
    public float timeToDetonate = 1.5f;
    public float damage = 50f;
    public float penetration = 100f;
    public GameObject explosionEffect;

    private void Update()
    {
        if (placed)
        {
            timeToDetonate -= Time.deltaTime;
        }

        if (timeToDetonate <= 0f)
        {
            Explode();
        }
    }

    private void Explode()
    {
        GameObject effectIns = Instantiate(explosionEffect, transform.position, transform.rotation);
        Destroy(effectIns, 5f);

        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        foreach (Collider collider in colliders)
        {
            if (collider.tag == "Enemy")
            {
                Damage(collider.transform);
            }
        }

        Destroy(gameObject);
    }

    private void Damage(Transform enemy)
    {
        EnemyBase e = enemy.GetComponent<EnemyBase>();
        e.TakeDamage(damage, penetration, Enums.Element.none);
    }
}
