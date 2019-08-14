using UnityEngine;

public class TowerLightning : TowerNonProjectile
{

    private Transform[] targets;

    public int maxTargets = 3;

    public GameObject lightning;
    public Transform firePoint;

    private GameObject[] enemies;
    private int targettingStyle;

    private float fireCountdown = 0;
    public string enemyTag = "Enemy";

    // Start is called before the first frame update
    protected void Start()
    {
        targets = new Transform[maxTargets];
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

    private void Update()
    {
        if (fireCountdown <= 0f)
        {
            Shoot();
            fireCountdown = 1f / triggerRate;
        }

        fireCountdown -= Time.deltaTime;
    }

    // runs GetTarget multiple times, depending on maxTargets, keeping an array of enemies to attack
    private void UpdateTarget()
    {
        // currentReference serves to indicate where the lightning will come from
        Transform currentReference = transform;
        enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        for (int i = 0; i < maxTargets; i++)
        {
            targets[i] = FindEnemy(currentReference);
            // uses the selected enemy as the point of reference for range
            currentReference = targets[i];
        }
    }

    // acquires a single target, making sure it doesn't get the same twice
    private Transform FindEnemy(Transform origin)
    {
        if (origin == null) return null;

        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;
        Transform target;
        int enemyIndex = -1;

        for (int i = 0; i < enemies.Length; i++)
        {
            // in case the enemy was already selected
            if (enemies[i] != null)
            {
                // calculates distance depending on the origin passed
                float distanceToEnemy = Vector3.Distance(origin.position, enemies[i].transform.position);
                if (distanceToEnemy < shortestDistance)
                {
                    shortestDistance = distanceToEnemy;
                    nearestEnemy = enemies[i];
                    enemyIndex = i;
                }
            }
        }

        if (nearestEnemy != null && shortestDistance <= range)
        {
            target = nearestEnemy.transform;
            // removes the enemy currently selected from the array of all enemies so it won't get selected again
            enemies[enemyIndex] = null;
        }
        else
        {
            target = null;
        }

        return target;
    }

    private void Shoot()
    {
        // currentReference serves to indicate where the lightning will come from
        Transform currentReference = firePoint;
        foreach (Transform target in targets)
        {
            if (target != null)
            {
                Damage(target, currentReference);
                // uses the selected enemy as the point of reference to create a LineRenderer between enemies
                currentReference = target;
            }
        }
    }

    void Damage(Transform enemy, Transform origin)
    {
        // generates a very short lived LineRenderer to simulate a (very lame) lightning strike
        GameObject chain = Instantiate(lightning, enemy.position, Quaternion.identity);
        LineRenderer chainRenderer = chain.GetComponent<LineRenderer>();
        chainRenderer.SetPosition(0, origin.position);
        chainRenderer.SetPosition(1, enemy.position);
        Destroy(chainRenderer, 0.1f);
        Destroy(chain, 0.1f);

        EnemyBase e = enemy.GetComponent<EnemyBase>();
        e.TakeDamage(damage, penetration, debuffElement);
    }

    protected override void UpgradeStatus()
    {
        range += rangeUpgrade;
    }
}
