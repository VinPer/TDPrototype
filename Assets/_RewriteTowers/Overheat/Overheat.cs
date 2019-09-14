using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Overheat : TowerBase
{
    public float fireRate;
    private float initialFireRate;
    private float fireCountdown = 0f;
    private Transform firePoints;
    public Transform rotationPart;
    private Transform[] fps = new Transform[6];
    public int poolAmount = 4;
    protected List<GameObject> bullets;
    public GameObject bulletPrefab;

    //Projectiles things
    public float damage = 10f;
    [Range(0f, 1f)]
    public float penetration = 0f;
    public float decayTimer = 2f;
    public float debuffIntensity = 0f;
    public float debuffDuration = 0f;
    public int projectileDurability = 1;

    public float damageUpgrade = .1f;
    public float fireRateUpgrade = .1f;
    public float debuffIntensityUpgrade = 10f;
    public float debuffDurationUpgrade = 1f;

    protected override void Awake()
    {
        base.Awake();
        initialFireRate = fireRate;
        SpawnBulletPool();
    }

    protected virtual void SpawnBulletPool()
    {
        bullets = new List<GameObject>();
        for (int i = 0; i < poolAmount; i++)
        {
            GameObject obj = (GameObject)Instantiate(bulletPrefab);
            obj.transform.SetParent(transform);
            obj.GetComponent<ProjectileBase>().debuffElement = element;
            obj.SetActive(false);
            bullets.Add(obj);
        }
    }
    protected void Start()
    {
        firePoints = rotationPart.GetChild(0);
        for (int i = 0; i < fps.Length; i++)
        {
            fps[i] = firePoints.GetChild(i).GetComponent<Transform>();
        }
        GetComponent<SphereCollider>().radius = range;
        //possibleTargets = new List<GameObject>();
    }
    protected void Update()
    {
        //if (possibleTargets.Count == 0) return;
        if (fireCountdown <= 0f)
        {
            Shoot();
            fireCountdown = 1f / fireRate;
        }

        fireCountdown -= Time.deltaTime;
    }
    

    protected void Shoot()
    {
        foreach(Transform fp in fps)
        {
            for (int i = 0; i < bullets.Count; i++)
            {
                if (!bullets[i].activeInHierarchy)
                {
                    Transform t = bullets[i].transform;
                    ProjectileBase p = bullets[i].GetComponent<ProjectileBase>();
                    //t.position = new Vector3(fp.position.x,possibleTargets[0].transform.position.y,fp.position.z);
                    t.position = new Vector3(fp.position.x, fp.position.y, fp.position.z);
                    t.rotation = fp.rotation;
                    bullets[i].SetActive(true);
                    //p.SetTarget(possibleTargets[0].transform);
                    Vector3 dir = fp.GetChild(0).transform.position - t.position;
                    dir.y = 0;
                    p.SetDirection(dir);

                    UpdateBulletStatus(p);

                    if (p.durability < projectileDurability)
                        p.durability += projectileDurability;

                    //Debug.Log(p.GetDirection());
                    break;
                }
            }
        }
        rotationPart.Rotate(new Vector3(0, 30, 0));
        if (GetComponentInParent<AudioSource>())
            GetComponentInParent<AudioSource>().Play();
        //List<GameObject> backup = new List<GameObject>(possibleTargets);
        //foreach (GameObject item in backup)
        //{
        //    if (!item.gameObject.activeSelf)
        //        possibleTargets.Remove(item);
        //}
    }
    protected void UpdateBulletStatus(ProjectileBase bullet)
    {
        bullet.damage = damage;
        bullet.penetration += penetration;
        
        bullet.debuffIntensity = debuffIntensity;
        bullet.debuffDuration = debuffDuration;
        bullet.decayTimer = decayTimer;
    }
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
        if (upgrades[_damage] < UpgradeHandler.data.towerUpgrades[transform.parent.gameObject.name][_damage])
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
        string _dIntensity = "debuffIntensity";
        if (upgrades[_dIntensity] < UpgradeHandler.data.towerUpgrades[transform.parent.name][_dIntensity])
        {
            debuffIntensity += debuffIntensityUpgrade;
            upgrades[_dIntensity]++;
        }
        string _dDuration = "debuffDuration";
        if (upgrades[_dDuration] < UpgradeHandler.data.towerUpgrades[transform.parent.name][_dDuration])
        {
            debuffDuration += debuffDurationUpgrade;
            upgrades[_dDuration]++;
        }
    }
}
