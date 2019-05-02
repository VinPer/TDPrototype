using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WaveSpawner : MonoBehaviour
{
    public static int EnemiesAlive = 0;

    public Wave[] waves;

    [Header("Enemies")]

    public Transform defaultEnemyPrefab;
    public Transform zipEnemyPrefab;
    public Transform armoredEnemyPrefab;

    [Header("Other")]

    public Transform spawnPoint;
    public Transform waypointHandler;

    public float timeBetweenWaves = 5f;
    public float timeBetweenBursts = 2f;
    public Text waveCountdownText;
    public Text waveNumber;

    private float countdown = 2f;
    private int waveIndex = 0;

    private void Start()
    {
        EnemiesAlive = 0;
    }

    private void Update()
    {
        waveNumber.text = EnemiesAlive + " - " + waveIndex;
        if (EnemiesAlive > 0) return;

        if (countdown <= 0f)
        {
            StartCoroutine(SpawnWave());
            countdown = timeBetweenWaves;
            return;
        }

        countdown -= Time.deltaTime;
        countdown = Mathf.Clamp(countdown, 0f, Mathf.Infinity);
        waveCountdownText.text = string.Format("{0:00.00}", countdown);
        waveNumber.text = EnemiesAlive + " - " + waveIndex;
    }

    // todo: implement concept of waves & bursts more elegantly
    IEnumerator SpawnWave()
    {
        PlayerStats.WavesSurvived++;
        int currentBurst = waveIndex % 4;
        waveIndex++;
        
        for (int i = 0; i < waveIndex; i++)
        {
            StartCoroutine(SpawnBurst(currentBurst));
            yield return new WaitForSeconds(timeBetweenBursts + (waveIndex % 2));
        }

        // todo: implement ending the level once you finish last wave
        //if (waveIndex == waves.Length)
        //{
        //    Debug.Log("Level won!");
        //    this.enabled = false;
        //}
    }

    IEnumerator SpawnBurst(int index)
    {
        Wave burst = waves[index];
        for(int i = 0; i < burst.count; i++)
        {
            SpawnEnemy(burst.enemy.transform);
            yield return new WaitForSeconds(burst.rate);
        }
    }

    void SpawnEnemy(Transform enemy)
    {
        Transform e = Instantiate(enemy, spawnPoint.position, spawnPoint.rotation);
        e.SetParent(transform);
        EnemiesAlive++;
    }

    public Transform[] GetWaypoints()
    {
        Transform[] waypoints = waypointHandler.GetComponent<Waypoints>().GetWaypoints();
        return waypoints;
    }
}
