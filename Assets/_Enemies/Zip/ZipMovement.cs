using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Enemy))]
public class ZipMovement : EnemyMovement
{
    public float delay = 1.5f;
    private bool canMove = false;

    private float initialDistance;

    protected override void Start()
    {
        base.Start();
        initialDistance = Vector3.Distance(transform.position, target.position);
        InvokeRepeating("Zip", 0.1f, delay);
    }

    protected override void Update()
    {
        if (canMove)
        {
            Vector3 direction = target.position - transform.position;
            float distance = direction.magnitude / initialDistance;
            if (distance > 1) distance = 1;
            transform.Translate(direction.normalized * enemy.speed * Time.deltaTime * distance, Space.World);
        }
        if (Vector3.Distance(transform.position, target.position) <= 0.5f)
        {
            GetNextWaypoint();
            canMove = false;
        }
    }

    private void Zip()
    {
        canMove = true;
    }

    protected override void GetNextWaypoint()
    {
        base.GetNextWaypoint();
        initialDistance = Vector3.Distance(transform.position, target.position);
    }
}
