using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    protected Transform[] waypoints;
    protected string waypointTag;
    [HideInInspector]
    public int waypointIndex;
    [HideInInspector]
    public float distToNextWaypoint;
    protected EnemyBase enemy;
    protected Transform target;
    protected Waypoint targetWaypoint;
    protected Vector3 targetPosition;
    public bool infiniteOffsets = true;
    protected bool stopOffset = false;

    protected float offsetX = 0;
    protected float offsetZ = 0;

    private float turnSpeed = 11f;

    private Vector3 spawnPosition;

    private void Awake()
    {
        spawnPosition = transform.position;
    }

    protected virtual void OnEnable()
    {
        if (waypoints == null) return;
        target = waypoints[waypointIndex];
        targetWaypoint = target.GetComponent<Waypoint>();
        GetNextPosition();
    }

    private void OnDisable()
    {
        SetWaypoint(0);
    }

    protected virtual void Start()
    {
        enemy = GetComponent<EnemyBase>();
        waypoints = transform.GetComponentInParent<WaveSpawner>().GetWaypoints();
        target = waypoints[waypointIndex];
        targetWaypoint = target.GetComponent<Waypoint>();
        GetNextPosition();
    }   

    protected virtual void Update()
    {
        Vector3 direction = targetPosition - transform.position;
        transform.Translate(direction.normalized * enemy.speed * Time.deltaTime, Space.World);

        if (Vector3.Distance(transform.position, targetPosition) <= 0.4f)
        {
            GetNextWaypoint();
        }
        distToNextWaypoint = Vector3.Distance(transform.position, waypoints[waypointIndex].position);
        LockOnTarget();
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
        //transform.LookAt(target);

    }

    protected virtual void LockOnTarget()
    {
        // Target lock on for nearest target
        Vector3 dir = targetPosition - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
        transform.rotation = Quaternion.Euler(0f, rotation.y, 0f);
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
        enemy.Hide();
        ReturnToSpawn();
    }

    public int GetWaypointIndex()
    {
        return waypointIndex;
    }

    public void SetWaypoint(int index)
    {
        waypointIndex = index;
    }

    public void ReturnToSpawn()
    {
        transform.position = spawnPosition;
    }
}
