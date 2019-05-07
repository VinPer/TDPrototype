using UnityEngine;

public class LightningChain : Turret
{

    private Transform[] targets;

    public int maxTargets = 3;
    public float damage = 15f;
    public float piercingValue = 0f;

    public GameObject lightning;

    private GameObject[] enemies;

    // Start is called before the first frame update
    protected override void Start()
    {
        targets = new Transform[maxTargets];
        base.Start();
    }

    protected override void Update()
    {
        if (fireCountdown <= 0f)
        {
            Shoot();
            fireCountdown = 1f / fireRate;
        }

        fireCountdown -= Time.deltaTime;
    }

    // runs GetTarget multiple times, depending on maxTargets, keeping an array of enemies to attack
    protected override void UpdateTarget()
    {
        // currentReference serves to indicate where the lightning will come from
        Transform currentReference = transform;
        enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        for (int i = 0; i < maxTargets; i++)
        {
            targets[i] = GetTarget(currentReference);
            // uses the selected enemy as the point of reference for range
            currentReference = targets[i];
        }
    }

    // acquires a single target, making sure it doesn't get the same twice
    private Transform GetTarget(Transform origin)
    {
        if (origin == null) return null;
        
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;
        Transform target;
        int enemyIndex = -1;

        for(int i = 0; i < enemies.Length; i++)
        {
            // in case the enemy was already selected
            if(enemies[i] != null)
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

    protected override void Shoot()
    {
        // currentReference serves to indicate where the lightning will come from
        Transform currentReference = firePoint;
        foreach(Transform target in targets)
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

        Enemy e = enemy.GetComponent<Enemy>();
        e.TakeDamage(damage, piercingValue);
    }
}
