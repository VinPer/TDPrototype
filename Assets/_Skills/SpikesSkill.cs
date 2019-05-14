using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikesSkill : Skill
{
    public float damage = 15f;
    public float piercing = 0f;
    public int ticks = 4;

    public string enemyTag = "Enemy";

    private bool placed = false;

    protected override void ActivateSkill()
    {
        placed = true;
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.tag != enemyTag || !placed) return;

        Enemy e = col.GetComponent<Enemy>();
        e.TakeDamage(damage, piercing);
        ticks--;

        if (ticks <= 0)
        {
            Destroy(gameObject);
        }
    }
}
