using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBase : MonoBehaviour
{
    [HideInInspector]
    public bool isDead;

    public float health = 100;
    private float initialHp;

    public float value = 10;
    [Range(0f,1f)]
    public float armor = 0; //from 0 to 1
    private float initialArmor;

    public float speed = 10;
    private float initialSpeed;

    public float damage = 1;





    //Experimental Recharching Shield
    [Range(0f,100f)] public float shieldCapacity = 0f;
    private float initialShieldCapacity;
    public float shieldRechargeRate = 25f;
    public float shieldRechargeDelay = 2f;
    private float initalShieldRechargeDelay;


    //End of Experimental Recharging Shield




    public bool invisible;
    
    private Enums.Status status = Enums.Status.disable;
    private Debuff fire;
    private Debuff slow;
    private Debuff acid;
    private Dictionary<Enums.Element, Debuff> debuffs;

    public Enums.Element element = Enums.Element.none;
    [Range(0f, 1f)]
    public float resistance = 0;

    public Image healthBar;
    public Image armorBar;
    public Image shieldBar;

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
    
    private void OnEnable()
    {
        isDead = false;
        status = Enums.Status.enable;
    }

    private void OnDisable()
    {
        GetComponent<EnemyMovement>().SetWaypoint(0);
    }

    protected virtual void Start()
    {
        //EXPERIMENTAL RECHARGING SHIELD
        initialShieldCapacity = shieldCapacity;
        initalShieldRechargeDelay = shieldRechargeDelay;
        //END OF EXPERIMENTAL RECHARGING SHIELD
        initialHp = health;
        initialSpeed = speed;

        if (armor > 1) armor = 1;
        initialArmor = armor;


        fire = new Debuff(true);
        acid = new Debuff(false);
        slow = new Debuff(false);

        debuffs = new Dictionary<Enums.Element, Debuff>
        {
            { Enums.Element.fire, fire },
            { Enums.Element.acid, acid },
            { Enums.Element.ice, slow }
        };
        Zero();
    }

    protected virtual void Update()
    {
        rechargeShield();
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
            TakeDamage(damage, 0, Enums.Element.fire);
            fire.duration--; // this needs to be improved with a time-relevant setting
            yield return new WaitForSeconds(0.2f); // delay between damage ticks
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
            armorBar.fillAmount = armor / initialArmor;
            yield return null;
        }
        Destroy(debuffEffect);
        armor = defaultArmor;
        armorBar.fillAmount = armor / initialArmor;
        acid.isActive = false;
    }

    public virtual void ActivateDebuff(float multiplier, float duration, Enums.Element debuffType)
    {
        if (status == Enums.Status.disable || debuffs == null) return;

        if (debuffType == element) return;

        Debuff debuff = debuffs[debuffType];
        debuff.Refresh(multiplier, duration);
        if (!debuff.isActive)
        {
            debuff.isActive = true;
            switch (debuffType)
            {
                case Enums.Element.fire:
                    StartCoroutine(ApplyFire());
                    break;
                case Enums.Element.acid:
                    StartCoroutine(ApplyAcid());
                    break;
                case Enums.Element.ice:
                    StartCoroutine(ApplySlow());
                    break;
            }
        }
    }

    public float GetHp() { return health; }
    public float GetDamage() { return damage; }
    public Enums.Status GetStatus() { return status; }
    public bool GetInvisibleState() { return invisible; }

    public void UpdateStatus()
    {
        if (status == Enums.Status.disable) status = Enums.Status.enable;
        else status = Enums.Status.disable;
    }
    public void UpdateInvisible(bool update)
    {
        invisible = update;
    }

    protected virtual void Die()
    {
        PlayerStats.Money += (int) value;
        PlayerStats.UpdateMoney();
        GameObject effect = Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(effect, 2f);

        StartCoroutine(Effect.PlayEffect(deathEffect,transform));
        
        if (debuffEffect != null) Destroy(debuffEffect);
        Hide();
    }

    public void Hide()
    {
        WaveSpawner.EnemiesAlive.Remove(gameObject);
        WaveSpawner.numberOfEnemiesAlive--;
        if (status == Enums.Status.disable) return;
        status = Enums.Status.disable;
        Zero();
        isDead = true;
        gameObject.SetActive(false);
    }

    public void Zero()
    {
        //EXPERIMENTAL RECHARGING SHIELD
        shieldCapacity = initialShieldCapacity;
        shieldRechargeDelay = initalShieldRechargeDelay;
        shieldBar.fillAmount = shieldCapacity / initialShieldCapacity;
        //END OF EXPERIMENTAL RECHARGING SHIELD
        health = initialHp;
        speed = initialSpeed;
        armor = initialArmor;
        foreach (Debuff debuff in debuffs.Values) debuff.Zero();
        healthBar.fillAmount = health / initialHp;
        armorBar.fillAmount = armor / initialArmor;
    }
    

    public virtual void TakeDamage(float amount, float piercingValue, Enums.Element turretElement)
    {
        if (element == turretElement) amount *= (1 - resistance);
        float multiplier = armor * (1 - piercingValue);
        //health -= amount * (1 - multiplier);
        ApplyDamage(amount, multiplier);
        healthBar.fillAmount = health / initialHp;
        if (health <= 0) Die();
        healthBar.fillAmount = health / initialHp;
    }

    //EXPERIMENTAL RECHARGING SHIELD
    protected virtual void ApplyDamage(float amount, float multiplier){
        float dmg = amount * (1 - multiplier);
        float shieldOverflow = dmg - shieldCapacity;

        shieldRechargeDelay = initalShieldRechargeDelay;

        if (shieldOverflow <= 0)
            shieldCapacity -= dmg;
        else
        {
            shieldCapacity = 0f;
            health -= shieldOverflow;
        }
        
        shieldBar.fillAmount = shieldCapacity / initialShieldCapacity;
    }

    private void rechargeShield(){

        if (shieldCapacity < initialShieldCapacity){

            shieldBar.fillAmount = shieldCapacity / initialShieldCapacity;
            shieldRechargeDelay -= Time.deltaTime;

            if (shieldRechargeDelay <= 0f){
                shieldCapacity += shieldRechargeRate * Time.deltaTime;
                //Mathf.Clamp(shieldCapacity, 0, initialShieldCapacity);
            }
        }
        
        if (shieldCapacity >= initialShieldCapacity){
            shieldRechargeDelay = initalShieldRechargeDelay;
            shieldCapacity = initialShieldCapacity;
            }
    }

    //END OF EXPERIMENTAL RECHARGING SHIELD
}
