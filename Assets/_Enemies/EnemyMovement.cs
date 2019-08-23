using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class AEnemyMovement : MonoBehaviour
{
    protected Enemy enemy;
    protected Transform target;
    protected Vector3 targetPosition;
    protected int waypointIndex = 0;
    protected Transform[] waypoints;
    protected Waypoint targetWaypoint;

    //public float waypointOffsetX = 2.5f;
    //public float waypointOffsetY = 2.5f;

    protected float offsetX = 0;
    protected float offsetZ = 0;

    public bool infiniteOffsets = true;
    protected bool stopOffset = false;

    // todo: improve offset handling
    //       includes targetPosition, offsetX, offsetZ
    protected virtual void Start()
    {
        waypoints = transform.GetComponentInParent<WaveSpawner>().GetWaypoints();
        target = waypoints[waypointIndex];
        targetWaypoint = target.GetComponent<Waypoint>();
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
        targetWaypoint = target.GetComponent<Waypoint>();
        GetNextPosition();
    }

    protected void GetNextPosition()
    {
        if (!stopOffset)
        {
            offsetX = Random.Range(-targetWaypoint.offsetX, targetWaypoint.offsetX);
            offsetZ = Random.Range(-targetWaypoint.offsetZ, targetWaypoint.offsetZ);
        }

        if (!infiniteOffsets) stopOffset = true;

        targetPosition = target.position + new Vector3(offsetX, 0f, offsetZ);
    }

    protected void EndPath()
    {
        PlayerStats.Lives--;
        PlayerStats.UpdateLives();
        Destroy(gameObject);
        WaveSpawner.numberOfEnemiesAlive--;
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
