using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerCharger : TowerProjectile
{
    public int maxChargeLevel = 5;
    public int maxChargeUpgrade = 1;
    private int currentChargeLevel = 1;
    public float chargeRate = 0.5f;
    public float chargeRateUpgrade = 0.5f;
    private float chargeCooldown = 0f;

    public AudioSource charging;
    public AudioSource shoot;
    public GameObject chargingFX;
    private GameObject chargingFXGO;

    private bool hasShot = false; 
    public float chargingFXRate;
    private float initialChargingFXRate;

    protected override void Start(){
        base.Start();
        chargeCooldown = 1 / chargeRate;
        chargingFXGO = Instantiate(chargingFX, firePoint.position, firePoint.rotation);
        InvokeRepeating("UpdateFXPosition", 0f, .5f);
        chargingFXRate = 1f/fireRate * 2f;
        initialChargingFXRate = chargingFXRate;
    }

    protected override void Update()
    {
        updateChargingFX();

        if (currentChargeLevel < maxChargeLevel && chargeCooldown <= 0f)
        {
            chargeCooldown = 1f / chargeRate;
            currentChargeLevel++;
            if (!charging.isPlaying) charging.Play();
        }

        chargeCooldown -= Time.deltaTime;

        if (target == null) return;

        LockOnTarget();

        if (fireCountdown <= 0f)
        {
            Shoot();
            currentChargeLevel = 1;
            fireCountdown = 1f / fireRate;
            chargeCooldown = 1f / chargeRate;
        }

        fireCountdown -= Time.deltaTime;
    }

    private void updateChargingFX(){

        chargingFXRate -= Time.deltaTime;
            
        if (chargingFXRate < 0f && hasShot)
        {
            chargingFXRate = initialChargingFXRate;
            print("activating");
            chargingFXGO.SetActive(true);
            hasShot = false;
        }
    }

    protected override void Shoot()
    {
        // base.Shoot();
        charging.Stop();
        //GameObject bulletGO = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        for (int i = 0; i < bullets.Count; i++)
        {
            if (!bullets[i].activeInHierarchy)
            {
                GameObject _bullet = bullets[i];
                ProjectileBase projectile = _bullet.GetComponent<ProjectileBase>();
                _bullet.transform.position = firePoint.position;
                _bullet.transform.rotation = firePoint.rotation;
                _bullet.SetActive(true);
                UpdateBulletStatus(projectile);
                projectile.SetTarget(target);
                shoot.Play();
                break;
            }
        }

        hasShot = true;
        chargingFXRate = initialChargingFXRate;
        chargingFXGO.SetActive(false);
    }

    protected override void UpdateBulletStatus(ProjectileBase projectile)
    {
        base.UpdateBulletStatus(projectile);
        projectile.SetDamage(projectile.GetDamage() * currentChargeLevel);
        projectile.SetExplosionRadius(currentChargeLevel);
        projectile.transform.localScale = projectile.GetInitialSize()* (1 + currentChargeLevel * .25f);
    }

    private IEnumerator RestartChargingFX()
    {
        chargingFX.SetActive(false);
        yield return new WaitForSeconds(1f);
        chargingFX.SetActive(true);
    }

    private void UpdateFXPosition()
    {
        chargingFXGO.transform.position = firePoint.position;
    }
    //FALTA UM JEITO DE JOGAR CHARGE RATE E MAX CHARGE PRO TIRO

    protected override void UpgradeStatus()
    {
        string _range = "range";
        if (upgrades[_range] < UpgradeHandler.data.towerUpgrades[transform.parent.name][_range])
        {
            range += rangeUpgrade;
            upgrades[_range]++;
            GetComponent<SphereCollider>().radius = range;
            print("range upgraded");
        }

        string _damage = "damage";
        if (upgrades[_damage] < UpgradeHandler.data.towerUpgrades[transform.parent.name][_damage])
        {
            damage += damageUpgrade;
            upgrades[_damage]++;
            print("damage upgraded");
        }

        string _chargeRate = "chargeRate";
        if (upgrades[_chargeRate] < UpgradeHandler.data.towerUpgrades[transform.parent.name][_chargeRate])
        {
            chargeRate += chargeRateUpgrade;
            upgrades[_chargeRate]++;
            print("chargeRate upgraded");
        }

        string _maxCharge = "maxCharge";
        if (upgrades[_maxCharge] < UpgradeHandler.data.towerUpgrades[transform.parent.name][_maxCharge])
        {
            maxChargeLevel += maxChargeUpgrade;
            upgrades[_maxCharge]++;
            print("maxCharge upgraded");
        }
    }

    private void OnDestroy()
    {
        Destroy(chargingFXGO);
    }
}
