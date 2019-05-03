using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyMovement : MonoBehaviour
{
    protected Enemy enemy;
    protected Transform target;
    protected Vector3 targetPosition;
    protected int waypointIndex = 0;
    protected Transform[] waypoints;
    
    public float waypointOffsetX = 2.5f;
    public float waypointOffsetY = 2.5f;

    protected float offsetX = 0;
    protected float offsetZ = 0;

    // todo: improve offset handling
    //       includes targetPosition, offsetX, offsetZ
    protected virtual void Start()
    {
        waypoints = transform.GetComponentInParent<WaveSpawner>().GetWaypoints();
        target = waypoints[waypointIndex];
        GetNextPosition();
        enemy = GetComponent<Enemy>();
    }

    protected virtual void Update()
    {
        Vector3 direction = targetPosition - transform.position;
        transform.Translate(direction.normalized * enemy.speed * Time.deltaTime, Space.World);

        if (Vector3.Distance(transform.position, targetPosition) <= 0.4f)
        {
            GetNextWaypoint();
        }
    }

    protected virtual void GetNextWaypoint()
    {
        if (waypointIndex >= waypoints.Length - 1)
        {
            EndPath();
            return;
        }

        waypointIndex++;
        target = waypoints[waypointIndex];
        GetNextPosition();
    }

    protected void GetNextPosition()
    {
        offsetX = Random.Range(-waypointOffsetX, waypointOffsetX);
        offsetZ = Random.Range(-waypointOffsetY, waypointOffsetY);
        targetPosition = target.position + new Vector3(offsetX, 0f, offsetZ);
    }

    protected void EndPath()
    {
        PlayerStats.Lives--;
        PlayerStats.UpdateLives();
        Destroy(gameObject);
        WaveSpawner.EnemiesAlive--;
    }
    
    public int GetWaypointIndex()
    {
        return waypointIndex;
    }

    public void SetWaypoint(int index)
    {
        waypointIndex = index;
    }
}
