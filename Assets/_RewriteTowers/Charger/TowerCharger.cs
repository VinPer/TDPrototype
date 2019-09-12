using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerCharger : TowerProjectile
{
    public int maxChargeLevel = 5;
    private int currentChargeLevel = 0;
    public float chargeRate = 0.5f;
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
            currentChargeLevel++;
            chargeCooldown = 1f / chargeRate;
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
                projectile.SetDamage(projectile.GetDamage() * currentChargeLevel);
                projectile.SetExplosionRadius(projectile.GetExplosionRadius() * currentChargeLevel);
                projectile.transform.localScale = projectile.transform.localScale * (1 + currentChargeLevel * .25f);
                projectile.SetTarget(target);
                shoot.Play();
                break;
            }
        }

        hasShot = true;
        chargingFXRate = initialChargingFXRate;
        chargingFXGO.SetActive(false);
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
}
