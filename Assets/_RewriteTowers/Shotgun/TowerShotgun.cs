using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerShotgun : TowerProjectile
{
    //Numero de tiros menores para spawnar
    public int pelletCount = 5;
    public int pelletCountUpgrade = 1;
    //angulo máximo do "spread" do tiro
    public float rotationOffset = 10f;
    public float rotationOffsetUpgrade = 1f;

    private List<GameObject> pellets;
    
    protected override void SpawnBulletPool()
    {
        //System.Random rng = new System.Random();
        //Quaternion rotation = Quaternion.Euler(new Vector3((float)rng.NextDouble() * rotationOffset, (float)rng.NextDouble() * rotationOffset, (float)rng.NextDouble() * rotationOffset));
        pellets = new List<GameObject>();
        
        for (int i = 0; i < pelletCount; i++)
        {
            GameObject newPellet = Instantiate(bulletPrefab, transform.position, transform.rotation);
            newPellet.transform.SetParent(transform);
            newPellet.SetActive(false);
            pellets.Add(newPellet);
        }
    }

    protected override void Shoot()
    {
        for (int i = 0; i < pellets.Count; i++)
        {
            if (!pellets[i].activeInHierarchy)
            {
                pellets[i].transform.position = firePoint.position;
                pellets[i].transform.rotation = firePoint.rotation;
                pellets[i].SetActive(true);
                UpdateBulletStatus(pellets[i].GetComponent<ProjectileBase>());
                pellets[i].GetComponent<ProjectileBase>().SetTarget(target);
            }
        }
        if (GetComponentInParent<AudioSource>())
            GetComponentInParent<AudioSource>().Play();
    }

    protected override void UpdateBulletStatus(ProjectileBase bullet)
    {
        base.UpdateBulletStatus(bullet);
        bullet.GetComponent<Pellet>().rotationOffset = rotationOffset;
    }

    //FALTA UM JEITO DE JOGAR PROJECTILE AMMOUNT E SPREAD REDUCTION PRO TIRO
    protected override void UpgradeStatus()
    {
        string _range = "range";
        if (upgrades[_range] < UpgradeHandler.data.towerUpgrades[transform.parent.name][_range])
        {
            range += rangeUpgrade;
            upgrades[_range]++;
            GetComponent<SphereCollider>().radius = range;
        }

        string _damage = "damage";
        if (upgrades[_damage] < UpgradeHandler.data.towerUpgrades[transform.parent.name][_damage])
        {
            damage += damageUpgrade;
            upgrades[_damage]++;
        }
        string _fireRate = "fireRate";
        if (upgrades[_fireRate] < UpgradeHandler.data.towerUpgrades[transform.parent.name][_fireRate])
        {
            fireRate += fireRateUpgrade;
            upgrades[_fireRate]++;
        }
        string _piercing = "piercing";
        if (upgrades[_piercing] < UpgradeHandler.data.towerUpgrades[transform.parent.name][_piercing])
        {
            penetration += penetrationUpgrade;
            upgrades[_piercing]++;
        }
        string _pAmount = "projectileAmount";
        if (upgrades[_pAmount] < UpgradeHandler.data.towerUpgrades[transform.parent.name][_pAmount])
        {
            pelletCount += pelletCountUpgrade;
            upgrades[_pAmount]++;
        }
        string _sReduction = "spreadReduction";
        if (upgrades[_sReduction] < UpgradeHandler.data.towerUpgrades[transform.parent.name][_sReduction])
        {
            rotationOffset -= rotationOffsetUpgrade;
            upgrades[_sReduction]++;
        }
    }
}
