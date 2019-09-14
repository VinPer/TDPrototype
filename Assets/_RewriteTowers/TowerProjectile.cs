using System.Collections.Generic;
using UnityEngine;

public class TowerProjectile : TowerBase
{

    //private Magazine magazine;
    public float fireRate = 1f;
    public float fireRateUpgrade = .2f;
    private float initialFireRate;
    protected Transform target;
    protected EnemyBase targetEnemy;
    
    public Transform partToRotate;
    public float turnSpeed = 10f;
    public Transform firePoint;
    protected float fireCountdown = 0f;

    protected List<GameObject> bullets;

    public List<string> targetStyles = new List<string> { "First", "Last", "Strongest", "Weakest" };
    public string targetSelected;

    protected List<GameObject> possibleTargets;

    [Header("Projectile things")]
    public GameObject bulletPrefab;
    public int poolAmount = 4;
    //Projectiles things
    public float damage = 10f;
    [Range(0f, 1f)]
    public float penetration = 0f;
    public float decayTimer = 2f;
    public int durability = 1;
    public float explosionRadius = 0f;
    public float debuffIntensity = 0f;
    public float debuffDuration = 0f;

    //Upgrades

    public float damageUpgrade = 1f;
    public float penetrationUpgrade = .1f;
    public float decayTimerUpgrade = .5f;
    public int durabilityUpgrade = 1;
    public int explosionRadiusUpgrade = 1;
    public float debuffIntensityUpgrade = 10f;
    public float debuffDurationUpgrade = 1f;

    //public enum TargetStyle { first, last, strongest, weakest };
    //public TargetStyle targetStyle = TargetStyle.first; //First by default

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

    protected virtual void Start()
    {
        GetComponent<SphereCollider>().radius = range;
        possibleTargets = new List<GameObject>();
        InvokeRepeating("UpdateTarget", 0f, 0.1f);
    }

    protected virtual void Update()
    {
        if (target == null) return;

        LockOnTarget();

        if (fireCountdown <= 0f)
        {
            Shoot();
            fireCountdown = 1f / fireRate;
        }

        fireCountdown -= Time.deltaTime;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        fireRate = initialFireRate;
        fireCountdown = 0;
        target = null;
        targetEnemy = null;
        //targetStyle = TargetStyle.first;
        targetSelected = targetStyles[0];
    }

    protected virtual void UpdateTarget()
    {
        target = null;
        targetEnemy = null;
        switch (targetSelected)
        {
            case ("First"):
                FindFirstTarget();
                break;
            case ("Last"):
                FindLastTarget();
                break;
            case ("Strongest"):
                FindStrongestTarget();
                break;
            case ("Weakest"):
                FindWeakestTarget();
                break;
        }
    }

    //The target is the first enemy
    private void FindFirstTarget()
    {
        target = null;
        targetEnemy = null;

        float shortestDist = Mathf.Infinity; //shortest distance to next waypoint
        int nextWaypoint = 0; //index of next waypoint

        List<GameObject> backup = new List<GameObject>(possibleTargets);

        foreach (GameObject e in backup)
        {
            if (!e.activeSelf) possibleTargets.Remove(e);
            else
            {
                EnemyBase enemy = e.GetComponent<EnemyBase>();
                if (seesInvisible || (!seesInvisible && !enemy.GetInvisibleState()))
                {
                    if (enemy.GetComponent<EnemyMovement>().GetWaypointIndex() > nextWaypoint)
                    {
                        nextWaypoint = enemy.GetComponent<EnemyMovement>().GetWaypointIndex();
                        target = enemy.transform;
                        targetEnemy = enemy;
                        shortestDist = enemy.GetComponent<EnemyMovement>().distToNextWaypoint;
                    }
                    if(enemy.GetComponent<EnemyMovement>().GetWaypointIndex() == nextWaypoint)
                    {
                        if (enemy.GetComponent<EnemyMovement>().distToNextWaypoint < shortestDist)
                        {
                            target = enemy.transform;
                            targetEnemy = enemy;
                            shortestDist = enemy.GetComponent<EnemyMovement>().distToNextWaypoint;
                        }
                    }
                }
            }
        }
    }

    private void FindLastTarget()
    {
        target = null;
        targetEnemy = null;

        float longestDist = 0; //shortest distance to next waypoint
        int lastWaypoint = 1000; //index of next waypoint

        List<GameObject> backup = new List<GameObject>(possibleTargets);

        foreach (GameObject e in backup)
        {
            if (!e.activeSelf) possibleTargets.Remove(e);
            else
            {
                EnemyBase enemy = e.GetComponent<EnemyBase>();
                if (seesInvisible || (!seesInvisible && !enemy.GetInvisibleState()))
                {
                    float distToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
                    if (distToEnemy <= range)
                    {
                        if (enemy.GetComponent<EnemyMovement>().GetWaypointIndex() < lastWaypoint)
                        {
                            lastWaypoint = enemy.GetComponent<EnemyMovement>().GetWaypointIndex();
                            target = enemy.transform;
                            targetEnemy = enemy;
                            longestDist = enemy.GetComponent<EnemyMovement>().distToNextWaypoint;
                        }
                        if (enemy.GetComponent<EnemyMovement>().GetWaypointIndex() == lastWaypoint)
                        {
                            if (enemy.GetComponent<EnemyMovement>().distToNextWaypoint < longestDist)
                            {
                                target = enemy.transform;
                                targetEnemy = enemy;
                                longestDist = enemy.GetComponent<EnemyMovement>().distToNextWaypoint;
                            }
                        }
                    }
                }
            }
        }
    }

    //The target is the enemy with the highest HP
    private void FindStrongestTarget()
    {
        target = null;
        targetEnemy = null;

        float highestHp = 0;

        List<GameObject> backup = new List<GameObject>(possibleTargets);

        foreach (GameObject e in backup)
        {
            if (!e.activeSelf) possibleTargets.Remove(e);
            else
            {
                EnemyBase enemy = e.GetComponent<EnemyBase>();
                if (seesInvisible || (!seesInvisible && !enemy.GetInvisibleState()))
                {
                    float distToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
                    if (distToEnemy <= range)
                    {
                        if (highestHp < enemy.GetHp())
                        {
                            target = enemy.transform;
                            targetEnemy = enemy;
                            highestHp = enemy.GetHp();
                        }
                    }
                }
            }
        }
    }
    //The target is the enemy with the lowest HP
    private void FindWeakestTarget()
    {
        target = null;
        targetEnemy = null;

        float lowestHp = Mathf.Infinity;

        List<GameObject> backup = new List<GameObject>(possibleTargets);

        foreach (GameObject e in backup)
        {
            if (!e.activeSelf) possibleTargets.Remove(e);
            else
            {
                EnemyBase enemy = e.GetComponent<EnemyBase>();
                if (seesInvisible || (!seesInvisible && !enemy.GetInvisibleState()))
                {
                    float distToEnemy = Vector3.Distance(transform.position, e.transform.position);
                    if (distToEnemy <= range)
                    {
                        if (lowestHp > enemy.GetHp())
                        {
                            target = e.transform;
                            targetEnemy = enemy;
                            lowestHp = enemy.GetHp();
                        }
                    }
                }
            }
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.GetComponent<EnemyBase>() && !possibleTargets.Contains(col.gameObject))
        {
            possibleTargets.Add(col.gameObject);
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (possibleTargets.Contains(col.gameObject))
            possibleTargets.Remove(col.gameObject);
    }

    public Transform GetTarget()
    {
        return target;
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
        targetEnemy = newTarget.GetComponent<EnemyBase>();
    }

    protected virtual void LockOnTarget()
    {
        // Target lock on for nearest target
        Vector3 dir = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
        partToRotate.rotation = Quaternion.Euler(0, rotation.y, 0);
    }

    protected virtual void Shoot()
    {
        ProjectileBase bullet;

        for (int i = 0; i < bullets.Count; i++)
        {
            if (!bullets[i].activeInHierarchy)
            {
                bullets[i].transform.position = firePoint.position;
                bullets[i].transform.rotation = firePoint.rotation;
                bullets[i].SetActive(true);

                bullet = bullets[i].GetComponent<ProjectileBase>();
                UpdateBulletStatus(bullet);

                bullet.SetTarget(target);

                if (GetComponentInParent<AudioSource>())
                    GetComponentInParent<AudioSource>().Play();

                break;
            }
        }
    }

    protected virtual void UpdateBulletStatus(ProjectileBase bullet)
    {
        bullet.damage = damage;
        bullet.penetration += penetration;
        bullet.durability = durability;
        bullet.explosionRadius = explosionRadius;
        bullet.debuffIntensity = debuffIntensity;
        bullet.debuffDuration = debuffDuration;
        bullet.decayTimer = decayTimer;
        bullet.debuffElement = element;
    }

    public void SetFireRate(float value)
    {
        if (value <= 0f) Debug.Log("Incorrect value to update fire rate!");
        else fireRate = value;
    }

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
        switch (transform.parent.name)
        {
            case "Basic":
                string _piercing = "piercing";
                if (upgrades[_piercing] < UpgradeHandler.data.towerUpgrades[transform.parent.name][_piercing])
                {
                    penetration += penetrationUpgrade;
                    upgrades[_piercing]++;
                    print("piercing upgraded");
                }

                string _penetration = "penetration";
                if (upgrades[_penetration] < UpgradeHandler.data.towerUpgrades[transform.parent.name][_penetration])
                {
                    durability += durabilityUpgrade;
                    upgrades[_penetration]++;
                    print("penetration upgraded");
                }

                string _fireRate = "fireRate";
                if (upgrades[_fireRate] < UpgradeHandler.data.towerUpgrades[transform.parent.name][_fireRate])
                {
                    fireRate += fireRateUpgrade;
                    upgrades[_fireRate]++;
                    print("fireRate upgraded");
                }
                break;
                
            case "Rocket":
                _fireRate = "fireRate";
                if (upgrades[_fireRate] < UpgradeHandler.data.towerUpgrades[transform.parent.name][_fireRate])
                {
                    fireRate += fireRateUpgrade;
                    upgrades[_fireRate]++;
                    print("fireRate upgraded");
                }
                string _radius = "explosionRadius";
                if (upgrades[_radius] < UpgradeHandler.data.towerUpgrades[transform.parent.name][_radius])
                {
                    explosionRadius += explosionRadiusUpgrade;
                    upgrades[_radius]++;
                    print("explosionRadius upgraded");
                }
                break;

            case "Sniper":
                _fireRate = "fireRate";
                if (upgrades[_fireRate] < UpgradeHandler.data.towerUpgrades[transform.parent.name][_fireRate])
                {
                    fireRate += fireRateUpgrade;
                    upgrades[_fireRate]++;
                    print("fireRate upgraded");
                }
                
                _penetration = "penetration";
                if (upgrades[_penetration] < UpgradeHandler.data.towerUpgrades[transform.parent.name][_penetration])
                {
                    durability += durabilityUpgrade;
                    upgrades[_penetration]++;
                    print("penetration upgraded");
                }
                break;    

            //UPGRADE CHARGER NO CHARGER


            case "Gatling":
                _piercing = "piercing";
                if (upgrades[_piercing] < UpgradeHandler.data.towerUpgrades[transform.parent.name][_piercing])
                {
                    penetration += penetrationUpgrade;
                    upgrades[_piercing]++;
                    print("piercing upgraded");
                }

                _penetration = "penetration";
                if (upgrades[_penetration] < UpgradeHandler.data.towerUpgrades[transform.parent.name][_penetration])
                {
                    durability += durabilityUpgrade;
                    upgrades[_penetration]++;
                    print("penetration upgraded");
                }

                _fireRate = "fireRate";
                if (upgrades[_fireRate] < UpgradeHandler.data.towerUpgrades[transform.parent.name][_fireRate])
                {
                    fireRate += fireRateUpgrade;
                    upgrades[_fireRate]++;
                    print("fireRate upgraded");
                }
                break;

            //FALTA UM JEITO DE JOGAR PUDDLE DURATION, PUDDLE SIZE PRO TIRO
            case "Spitter":
                string _pDuration = "puddleDuration";
                if (upgrades[_pDuration] < UpgradeHandler.data.towerUpgrades[transform.parent.name][_pDuration])
                {
                    for (int i = 0; i < transform.childCount; i++)
                    {
                        transform.GetChild(i).GetComponent<ProjectileSpitter>().puddleDuration += 1;
                    }
                    upgrades[_pDuration]++;
                    print("puddleDuration upgraded");
                }
                string _dIntensity = "debuffIntensity";
                if (upgrades[_dIntensity] < UpgradeHandler.data.towerUpgrades[transform.parent.name][_dIntensity])
                {
                    debuffIntensity += debuffIntensityUpgrade;
                    upgrades[_dIntensity]++;
                    print("debuffIntensity upgraded");
                }
                string _dDuration = "debuffDuration";
                if (upgrades[_dDuration] < UpgradeHandler.data.towerUpgrades[transform.parent.name][_dDuration])
                {
                    debuffDuration += debuffDurationUpgrade;
                    upgrades[_dDuration]++;
                    print("debuffDuration upgraded");
                }
                break;
            
            default:
                print("Nome de Torre Errado pro Upgrade!!!!!");
                break;
        }
    }

    protected void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
