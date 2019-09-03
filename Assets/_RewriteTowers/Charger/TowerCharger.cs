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

    protected override void Update()
    {
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
    }
}
