using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class WaveSpawnerOld : MonoBehaviour
{
    public static int EnemiesAlive = 0;

    public Wave[] waves;
    private Dictionary<int, List<GameObject>> enemyWave;
    private List<GameObject> enemiesOfWave;

    [Header("Enemies")]

    public Transform defaultEnemyPrefab;
    public Transform zipEnemyPrefab;
    public Transform armoredEnemyPrefab;

    [Header("Other")]

    public Transform spawnPoint;
    public Transform waypointHandler;
    public bool infiniteOffset = true;

    public float timeBetweenWaves = 5f;
    public float timeBetweenBursts = 2f;
    public Text waveCountdownText;
    public Text waveNumber;

    private float countdown = 2f;
    private int waveIndex = 0;
    public bool countingDown = true;



    private void Start()
    {
        //Pooling
        PoolEnemies();

        EnemiesAlive = 0;
    }

    void PoolEnemies()
    {
        enemyWave = new Dictionary<int, List<GameObject>>();
        enemiesOfWave = new List<GameObject>();

        for (int i = 0; i < waves.Length; i++)
        {
            Wave wave = waves[i];
            if(enemiesOfWave.Count>0)
                enemiesOfWave.Clear();
            for (int j = 0; j < wave.count; j++)
            {
                GameObject obj = (GameObject)Instantiate(wave.enemy, spawnPoint.position, spawnPoint.rotation);
                obj.transform.SetParent(transform);
                obj.SetActive(false);
                enemiesOfWave.Add(obj);
            }
            enemyWave.Add(i, enemiesOfWave);
        }
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

        if (countingDown)
        {
            countdown -= Time.deltaTime;
            countdown = Mathf.Clamp(countdown, 0f, Mathf.Infinity);
            waveCountdownText.text = string.Format("{0:00.00}", countdown);
            waveNumber.text = EnemiesAlive + " - " + waveIndex;
        }
    }

    // todo: implement concept of waves & bursts more elegantly
    IEnumerator SpawnWave()
    {
        PlayerStats.WavesSurvived++;
        print(PlayerStats.WavesSurvived);
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

    //IEnumerator SpawnBurst(int index)
    //{
    //    Wave burst = waves[index];
    //    for (int i = 0; i < burst.count; i++)
    //    {
    //        SpawnEnemy(burst.enemy.transform);
    //        yield return new WaitForSeconds(burst.rate);
    //    }
    //}


    //public void SpawnEnemy(Transform enemy)
    //{
    //    Transform e = Instantiate(enemy, spawnPoint.position, spawnPoint.rotation);
    //    e.SetParent(transform);
    //    e.GetComponent<EnemyMovement>().infiniteOffsets = infiniteOffset;
    //    EnemiesAlive++;
    //}

    IEnumerator SpawnBurst(int index)
    {
        Wave burst = waves[index];
        for (int i = 0; i < burst.count; i++)
        {
            SpawnEnemy(index + i);
            yield return new WaitForSeconds(burst.rate);
        }
    }

    //public void SpawnEnemy(int index)
    //{
    //    enemies[index].SetActive(true);
    //    enemies[index].GetComponent<EnemyMovement>().infiniteOffsets = infiniteOffset;
    //    EnemiesAlive++;
    //}

    public void SpawnEnemy(int index)
    {
        for (int i = 0; i < enemyWave[index].Count; i++)
        {
            GameObject enemyToSpawn = enemyWave[index][i];
            enemyToSpawn.SetActive(true);
            enemyToSpawn.GetComponent<EnemyMovement>().infiniteOffsets = infiniteOffset;
            EnemiesAlive++;
        }
    }

    public Transform[] GetWaypoints()
    {
        Transform[] waypoints = waypointHandler.GetComponent<Waypoints>().GetWaypoints();
        return waypoints;
    }
}
