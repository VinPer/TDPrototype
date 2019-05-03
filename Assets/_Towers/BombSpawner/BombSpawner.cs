using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombSpawner : MonoBehaviour
{
    public Transform spawnPoint;
    public GameObject bombPrefab;
    private GameObject bomb;

    public float timeBetweenSpawns = 30f;
    private float spawnCountdown = 2f;

    // Spawns a bomb timeBetweenSpawns seconds after the current bomb has exploded
    void Update()
    {
        if (bomb != null)
        {
            ;
        }
        else if (spawnCountdown <= 0f)
        {
            SpawnBomb();
            spawnCountdown = timeBetweenSpawns;
        }
        else
        {
            spawnCountdown -= Time.deltaTime;
        }
    }

    private void SpawnBomb()
    {
        bomb = (GameObject) Instantiate(bombPrefab, spawnPoint.position, spawnPoint.rotation);
    }
}
