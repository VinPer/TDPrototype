using UnityEngine;

public class SkillSpikes : SkillTargetted
{
    public float damage = 15f;
    public float penetration = 0f;
    public int ticks = 4;

    public string enemyTag = "Enemy";

    private void OnTriggerEnter(Collider col)
    {
        if (col.tag != enemyTag || !placed) return;

        Enemy e = col.GetComponent<Enemy>();
        e.TakeDamage(damage, penetration);
        ticks--;

        if (ticks <= 0)
        {
            Destroy(gameObject);
        }
    }
}
