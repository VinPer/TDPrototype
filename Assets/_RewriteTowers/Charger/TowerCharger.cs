using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerCharger : TowerProjectile
{

    public int maxChargeLevel = 3;
    private int currentChargeLevel = 0;
    public float chargeRate = 0.5f;
    private float chargeCooldown = 0f;
    
    protected override void Update()
    {
        if (currentChargeLevel < maxChargeLevel && chargeCooldown <= 0f)
        {
            currentChargeLevel++;
            chargeCooldown = 1f / chargeRate;
        }

        chargeCooldown -= Time.deltaTime;

        if (target == null) return;

        LockOnTarget();

        if (fireCountdown <= 0f)
        {
            Shoot();
            fireCountdown = 1f / fireRate;
        }

        fireCountdown -= Time.deltaTime;
    }

    protected override void Shoot()
    {
        // base.Shoot();
        GameObject bulletGO = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        ProjectileBase bullet = bulletGO.GetComponent<ProjectileBase>();

        if (bullet != null)
        {
            bullet.SetDamage(bullet.GetDamage() * currentChargeLevel);
            bullet.SetDurability(currentChargeLevel);
            // change scale of bullet
            bullet.SetTarget(target);
        }
    }
}
