using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCluster : EnemyBase
{
    [Header("Cluster Settings")]
    public int childrenToSpawn = 3;
    public float overflow = 15f; // overflow handles overkill damage in ClusterDeath()
    public GameObject child;

    private List<GameObject> children;

    private void Awake()
    {
        children = new List<GameObject>();
        for (int i = 0; i < childrenToSpawn; i++)
        {
            GameObject newChild = Instantiate(child, transform.position, transform.rotation);
            children.Add(newChild);
            newChild.transform.SetParent(transform.parent);
            newChild.SetActive(false);
        }
    }

    protected override void Start()
    {
        base.Start();
        foreach (GameObject _child in children) _child.transform.SetParent(transform.parent);
    }

    protected override void Die()
    {
        ClusterDeath();
        base.Die();
    }

    private void ClusterDeath()
    {
        // reduces amount of children to spawn if there's enough overkill damage
        if (health < 0)
        {
            float childHealth = child.GetComponent<EnemyBase>().health;
            int newChildrenToSpawn = Mathf.CeilToInt((overflow + health) / childHealth);
            value += child.GetComponent<EnemyBase>().value * (childrenToSpawn - newChildrenToSpawn);
            childrenToSpawn = newChildrenToSpawn;
        }

        for (int i = 0; i < childrenToSpawn; i++)
        {
            SpawnChild();
        }
    }

    //private void SpawnChild()
    //{
    //    GameObject c = Instantiate(child, transform.position, Quaternion.identity);
    //    c.transform.SetParent(transform.parent);

    //    // sets the waypoint of the child to be the same as the dying father's
    //    EnemyMovement childMovement = c.GetComponent<EnemyMovement>();
    //    EnemyMovement parentMovement = transform.GetComponent<EnemyMovement>();
    //    childMovement.SetWaypoint(parentMovement.GetWaypointIndex());
    //    WaveSpawner.numberEnemiesAlive++;
    //}
    private void SpawnChild()
    {
        for (int i = 0; i < children.Count; i++)
        {
            if (!children[i].activeInHierarchy)
            {
                children[i].transform.position = transform.position;
                children[i].transform.rotation = transform.rotation;
                children[i].GetComponent<EnemyMovement>().SetWaypoint(GetComponent<EnemyMovement>().GetWaypointIndex());
                children[i].SetActive(true);
                WaveSpawner.numberOfEnemiesAlive++;
                WaveSpawner.EnemiesAlive.Add(children[i]);
                // sets the waypoint of the child to be the same as the dying father's

                return;
            }
        }
    }

}
