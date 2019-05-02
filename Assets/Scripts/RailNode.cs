using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailNode : MonoBehaviour
{
    public Transform waypointHandler;
    public float speed = 3.5f;
    public bool circular = true;

    private Transform[] waypoints;
    private Transform target;
    private int waypointIndex = 0;
    private bool forwards = true;

    private void Start()
    {
        waypoints = waypointHandler.GetComponent<Waypoints>().GetWaypoints();
        target = waypoints[0];
    }

    private void Update()
    {
        Vector3 direction = target.position - transform.position;
        transform.Translate(direction.normalized * speed * Time.deltaTime, Space.World);

        if (Vector3.Distance(transform.position, target.position) <= 0.4f)
        {
            GetNextWaypoint();
        }
    }

    // this needs severe optimization
    private void GetNextWaypoint()
    {
        if (waypointIndex >= waypoints.Length - 1 || (waypointIndex <= 0 & !forwards))
        {
            if (circular) waypointIndex = 0;
            else
            {
                forwards = !forwards;
                if (forwards) waypointIndex++;
                else waypointIndex--;
            }
        }
        else
        {
            if (forwards) waypointIndex++;
            else waypointIndex--;
        }

        target = waypoints[waypointIndex];
    }
}
