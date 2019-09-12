using System.Collections.Generic;
using UnityEngine;

public class TowerProjectile : TowerBase
{
    //private Magazine magazine;
    public float fireRate = 1f;
    private float initialFireRate;
    protected Transform target;
    protected EnemyBase targetEnemy;
    
    public Transform partToRotate;
    public float turnSpeed = 10f;
    public Transform firePoint;
    protected float fireCountdown = 0f;

    public int poolAmount = 4;
    protected List<GameObject> bullets;
    public GameObject bulletPrefab;

    public float penetrationBoost = 0;
    public int projectileDurability = 1;

    public float damageUpgrade = .1f;
    public float fireRateUpgrade = .1f;

    //public enum TargetStyle { first, last, strongest, weakest };
    //public TargetStyle targetStyle = TargetStyle.first; //First by default
    public List<string> targetStyles = new List<string> {"First","Last","Strongest","Weakest" };
    public string targetSelected;

    protected List<GameObject> possibleTargets;

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
                    if (enemy.GetComponent<EnemyMovement>().GetWaypointIndex() >= nextWaypoint)
                    {
                        nextWaypoint = enemy.GetComponent<EnemyMovement>().GetWaypointIndex();
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
                        if (enemy.GetComponent<EnemyMovement>().GetWaypointIndex() <= lastWaypoint)
                        {
                            lastWaypoint = enemy.GetComponent<EnemyMovement>().GetWaypointIndex();
                            if (enemy.GetComponent<EnemyMovement>().distToNextWaypoint > longestDist)
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
        partToRotate.rotation = Quaternion.Euler(rotation.x, rotation.y, rotation.z);
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

                bullet.damage *= damageBoost;
                bullet.penetration += penetrationBoost;

                if(bullet.durability < projectileDurability)
                    bullet.durability += projectileDurability;

                bullet.SetTarget(target);

                if (GetComponentInParent<AudioSource>())
                    GetComponentInParent<AudioSource>().Play();

                break;
            }
        }
    }

    public void SetFireRate(float value)
    {
        if (value <= 0f) Debug.Log("Incorrect value to update fire rate!");
        else fireRate = value;
    }

    protected override void UpgradeStatus()
    {
        string _range = "range";
        if (upgrades[_range] < UpgradeHandler.data.towerUpgrades[gameObject.name][_range])
        {
            range += rangeUpgrade;
            upgrades[_range]++;
            GetComponent<SphereCollider>().radius = range;
        }

        string _damage = "damage";
        if (upgrades[_damage] < UpgradeHandler.data.towerUpgrades[gameObject.name][_damage])
        {
            damageBoost += damageUpgrade;
            upgrades[_damage]++;
        }
        switch (gameObject.name)
        {
            case "Basic":
                string _piercing = "piercing";
                if (upgrades[_piercing] < UpgradeHandler.data.towerUpgrades[gameObject.name][_piercing])
                {
                     penetrationBoost += .1f;
                    upgrades[_piercing]++;
                }

                string _penetration = "penetration";
                if (upgrades[_penetration] < UpgradeHandler.data.towerUpgrades[gameObject.name][_penetration])
                {
                    projectileDurability += 1;
                    upgrades[_penetration]++;
                }
                break;
        }
    }

    protected void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
