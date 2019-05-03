using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    protected float initialSpeed;
    public float speed = 10f;

    protected float initialHealth;
    public float health = 10f;
    public float armor = 0;
    public int damage = 1;
    public int value = 100;

    public GameObject deathEffect;
    public Image healthBar;

    protected Dictionary<string, Debuff> debuffs;
    protected Debuff fire;
    protected Debuff acid;
    protected Debuff slow;

    protected GameObject debuffEffect;
    public GameObject fireEffect;
    public GameObject acidEffect;
    public GameObject slowEffect;

    protected void Start()
    {
        initialSpeed = speed;
        initialHealth = health;

        fire = new Debuff(true);
        acid = new Debuff(false);
        slow = new Debuff(false);
        
        debuffs = new Dictionary<string, Debuff>
        {
            { "fire", fire },
            { "acid", acid },
            { "slow", slow }
        };
    }

    public void TakeDamage(float amount, float piercingValue)
    {
        float multiplier = (100 - piercingValue) * armor / 10000;
        health -= amount * (1 - multiplier);
        healthBar.fillAmount = health / initialHealth;
        if (health <= 0) Die();
    }

    protected virtual void Die()
    {
        PlayerStats.Money += value;
        PlayerStats.UpdateMoney();
        WaveSpawner.EnemiesAlive--;

        GameObject effect = Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(effect, 5f);

        if (debuffEffect != null) Destroy(debuffEffect);

        Destroy(gameObject);
    }
    // look into delegates so that debuffs can all have a default function that activates their
    // respective coroutines, passing the debuff in particular as a parameter?
    public void ActivateDebuff(float multiplier, float duration, string debuffType)
    {
        Debuff debuff = debuffs[debuffType];
        debuff.Refresh(multiplier, duration);
        if (!debuff.isActive)
        {
            debuff.isActive = true;
            switch (debuffType)
            {
                case "fire":
                    StartCoroutine(Fire());
                    break;
                case "acid":
                    StartCoroutine(Acid());
                    break;
                case "slow":
                    StartCoroutine(Slow());
                    break;
            }
        }
    }

    // todo: implement slow within a debuff list
    //       change slow to only count down after enemy is not being hit anymore
    protected IEnumerator Slow()
    {
        debuffEffect = Instantiate(slowEffect, transform.position, transform.rotation);
        debuffEffect.transform.SetParent(transform);
        while (slow.duration > 0)
        {
            speed = initialSpeed * (100f - slow.level) / 100; // checks this every time just in case it's updated
            slow.duration -= Time.deltaTime;
            yield return null;
        }
        Destroy(debuffEffect);
        speed = initialSpeed;
        slow.isActive = false;
    }

    protected IEnumerator Fire()
    {
        debuffEffect = Instantiate(fireEffect, transform.position, transform.rotation);
        debuffEffect.transform.SetParent(transform);
        float damage;
        while (fire.duration > 0)
        {
            damage = Mathf.Min(fire.level, 100) / 100;
            TakeDamage(damage, 0);
            fire.duration--; // this needs to be improved with a time-relevant setting
            yield return new WaitForSeconds(0.1f); // delay between damage ticks
        }
        Destroy(debuffEffect);
        fire.isActive = false;
    }

    protected IEnumerator Acid()
    {
        debuffEffect = Instantiate(acidEffect, transform.position, transform.rotation);
        debuffEffect.transform.SetParent(transform);
        float defaultArmor = armor;
        while (acid.duration > 0)
        {
            armor = armor * (100 - acid.level) / 100;
            acid.duration -= Time.deltaTime;
            yield return null;
        }
        Destroy(debuffEffect);
        armor = defaultArmor;
        acid.isActive = false;
    }
}
