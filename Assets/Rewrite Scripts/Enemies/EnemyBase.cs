using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBase : MonoBehaviour
{
    public float health = 100;
    private float initialHp;

    public float value = 10;
    public float armor = 0; //from 0 to 1
    private float initialArmor;

    public float speed = 10;
    private float initialSpeed;

    public float damage = 1;
    public bool invisible;

    public enum Status { enable, disable }
    private Status status;
    private Debuff fire;
    private Debuff slow;
    private Debuff acid;
    private Dictionary<Elements.Element, Debuff> debuffs;

    public Elements.Element element = Elements.Element.none;
    public float resistance = 0;

    public Image healthBar;
    public Image armorBar;

    //Effects
    [Header("Effects")]
    public GameObject fireEffect;
    public GameObject slowEffect;
    public GameObject acidEffect;
    private GameObject debuffEffect;
    public GameObject deathEffect;

    //Color
    protected Renderer rend;
    protected Color startColor;
    private Color invisibleColor;

    protected virtual void Start()
    {
        status = Status.disable;
        initialHp = health;
        initialSpeed = speed;

        if (armor > 1) armor = 1;
        initialArmor = armor;


        fire = new Debuff(true);
        acid = new Debuff(false);
        slow = new Debuff(false);

        debuffs = new Dictionary<Elements.Element, Debuff>
        {
            { Elements.Element.fire, fire },
            { Elements.Element.acid, acid },
            { Elements.Element.ice, slow }
        };
        Spawn();
    }
    public void Spawn()
    {
        status = Status.enable;
        health = initialHp;
        speed = initialSpeed;
        armor = initialArmor;
        healthBar.fillAmount = health / initialHp;
        armorBar.fillAmount = armor / initialArmor;

        rend = GetComponent<Renderer>();
        if (rend == null)
        {
            rend = GetComponentInChildren<Renderer>();
        }
        startColor = rend.material.color;
        invisibleColor = startColor;
        invisibleColor.a = 0f;
        if (invisible) rend.material.color = invisibleColor;
    }

    //DEBUFFS
    public class Debuff
    {
        public float duration;
        public float level;
        public bool isActive;
        public bool accumulates;

        public Debuff(bool accumulates)
        {
            duration = 0;
            level = 0;
            isActive = false;
            this.accumulates = accumulates;
        }

        public void Refresh(float level, float duration)
        {
            if (accumulates)
                this.level += level;

            else
                this.level = level;
            this.duration = duration;
        }

        public void Zero()
        {
            duration = 0;
            level = 0;
            isActive = false;
        }
    }

    protected IEnumerator ApplySlow()
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

    protected IEnumerator ApplyFire()
    {
        debuffEffect = Instantiate(fireEffect, transform.position, transform.rotation);
        debuffEffect.transform.SetParent(transform);
        float damage;
        while (fire.duration > 0)
        {
            damage = Mathf.Min(fire.level, 100) / 100;
            TakeDamage(damage, 0, Elements.Element.fire);
            fire.duration--; // this needs to be improved with a time-relevant setting
            yield return new WaitForSeconds(0.1f); // delay between damage ticks
        }
        Destroy(debuffEffect);
        fire.isActive = false;
    }

    protected IEnumerator ApplyAcid()
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

    public virtual void ActivateDebuff(float multiplier, float duration, Elements.Element debuffType)
    {
        if (status == Status.disable || debuffs == null) return;

        if (debuffType == element) return;

        Debuff debuff = debuffs[debuffType];
        debuff.Refresh(multiplier, duration);
        if (!debuff.isActive)
        {
            debuff.isActive = true;
            switch (debuffType)
            {
                case Elements.Element.fire:
                    StartCoroutine(ApplyFire());
                    break;
                case Elements.Element.acid:
                    StartCoroutine(ApplyAcid());
                    break;
                case Elements.Element.ice:
                    StartCoroutine(ApplySlow());
                    break;
            }
        }
    }

    public float GetHp() { return health; }
    public float GetDamage() { return damage; }
    public Status GetStatus() { return status; }
    public bool GetInvisibleState() { return invisible; }

    public void UpdateStatus()
    {
        if (status == Status.disable) status = Status.enable;
        else status = Status.disable;
    }
    public void UpdateInvisible()
    {
        invisible = !invisible;
        if (invisible) rend.material.color = invisibleColor;
        else rend.material.color = startColor;
    }

    protected virtual void Die()
    {
        if (status == Status.disable) return;
        status = Status.disable;
        PlayerStats.Money += (int) value;
        PlayerStats.UpdateMoney();
        WaveSpawner.EnemiesAlive--;

        GameObject effect = Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(effect, 5f);

        if (debuffEffect != null) Destroy(debuffEffect);

        //Sera alterado
        Destroy(gameObject);
    }

    public virtual void TakeDamage(float amount, float piercingValue, Elements.Element turretElement)
    {
        if (element == turretElement) amount *= (1 - resistance);

        float multiplier = armor * (1 - piercingValue);
        health -= amount * (1 - multiplier);
        healthBar.fillAmount = health / initialHp;
        if (health <= 0) Die();
        healthBar.fillAmount = health / initialHp;
        armorBar.fillAmount = armor / initialArmor;
    }
}
