using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveSpawner : MonoBehaviour
{
    [System.Serializable]
    public class Waves
    {
        public WaveBurst[] burst;
    }

    public static int numberOfEnemiesAlive = 0;

    public enum SpawnerState { spawning, waiting, counting };
    private SpawnerState state = SpawnerState.counting;

    public Waves[] waves;
    private Dictionary<GameObject, int> enemiesCount; //Count of each type of enemy
    private Dictionary<GameObject, List<GameObject>> enemiesAvailable; //Enemies used in level

    public static List<GameObject> EnemiesAlive;
    
    [Header("Other")]
    public Transform spawnPoint;
    public Transform[] waypointHandler;
    public bool infiniteOffset = true;

    public float timeBetweenWaves = 5f;

    private float waveCountdown = 2f;
    private int nextWave = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        EnemiesAlive = new List<GameObject>();
        numberOfEnemiesAlive = 0;

        //pool
        PoolEnemies();
        waveCountdown = timeBetweenWaves;
        state = SpawnerState.counting;
        //WavesLeft();
    }

    void PoolEnemies()
    {
        enemiesCount = new Dictionary<GameObject, int>();
        enemiesAvailable = new Dictionary<GameObject, List<GameObject>>();

        //Get max number of each enemy
        for (int i = 0; i < waves.Length; i++)
        {
            for (int j = 0; j < waves[i].burst.Length; j++)
            {
                WaveBurst burst = waves[i].burst[j];
                if (enemiesCount.ContainsKey(burst.enemyType))
                {
                    if(enemiesCount[burst.enemyType] < burst.count)
                    {
                        enemiesCount[burst.enemyType] = burst.count;
                    }
                }
                else
                {
                    enemiesCount.Add(burst.enemyType,burst.count);
                }
            }
        }
        foreach (GameObject enemyType in enemiesCount.Keys)
        {
            int n = 0;
            for (int i = 0; i < enemiesCount[enemyType]; i++)
            {
                GameObject obj = (GameObject)Instantiate(enemyType, spawnPoint.position, spawnPoint.rotation);
                obj.transform.SetParent(transform);
                obj.SetActive(false);
                if (enemiesAvailable.ContainsKey(enemyType))
                {
                    enemiesAvailable[enemyType].Add(obj);
                    n++;
                }
                else
                {
                    List<GameObject> objs = new List<GameObject>();
                    objs.Add(obj);
                    enemiesAvailable.Add(enemyType, objs);
                    n = 0;
                }
            }
        }
    }

    private void Update()
    {

        if (GameManager.GameIsOver) return;
        
        if (state == SpawnerState.waiting)
        {
            //check if enemies are still alive
            if (!EnemyIsAlive())
            {
                //begin a new round
                WaveCompleted();

                return;
            }
            else
            {
                return;
            }
        }

        if (waveCountdown <= 0)
        {
            waveCountdown = 0;
            if (state != SpawnerState.spawning)
            StartCoroutine(SpawnWave(waves[nextWave]));
        }
        else
        {
            waveCountdown -= Time.deltaTime;
            waveCountdown = Mathf.Clamp(waveCountdown, 0f, Mathf.Infinity);
            //waveNumber.text = EnemiesAlive + " - " + waveIndex;
        }

    }

    void WaveCompleted()
    {
        Debug.Log("Wave Completed!");
        if (nextWave + 1 > waves.Length -1)
        {
            enabled = false;
            if (!GameManager.GameIsOver)
            {
                Debug.Log("currently disabled");
                GameManager.instance.Winning();
            }
        }
        else
        {
            state = SpawnerState.counting;
            waveCountdown = timeBetweenWaves;
            nextWave++;
            //WavesLeft();
            WaveCount.instance.UpdateWaveCount();
        }

    }

    //void WavesLeft()
    //{
    //    if (nextWave + 1 > waves.Length)
    //    {
    //        wavesLeft.text = "Last wave!";
    //    }
    //    else
    //    {
    //        wavesLeft.text = (waves.Length - nextWave).ToString();
    //    }
    //}

    bool EnemyIsAlive()
    //check if enemies are alive
    {
        bool res = false;
        if (numberOfEnemiesAlive > 0)
        {
            res = true;
        }
        return res;
    }

    IEnumerator SpawnWave(Waves _wave)
    {
        PlayerStats.WavesSurvived++;
        state = SpawnerState.spawning;
        int count;
        float rate;
        foreach (WaveBurst enemy in _wave.burst)
        {
            count = enemy.count;
            if (count == 0)
                count = Random.Range(1, 10);
            rate = enemy.rate;
            if (rate == 0)
                rate = Random.Range(1, 5);  
            for (int i = 0; i < count; i++)
            {
                SpawnEnemy(enemy.enemyType);
                yield return new WaitForSeconds(1f / rate);
            }
        }

        state = SpawnerState.waiting;

        yield break;
    }

    void SpawnEnemy(GameObject _enemyType)
    {
        for (int i = 0; i < enemiesAvailable[_enemyType].Count; i++)
        {
            if (!enemiesAvailable[_enemyType][i].activeInHierarchy)
            {
                enemiesAvailable[_enemyType][i].transform.position = spawnPoint.position;
                enemiesAvailable[_enemyType][i].transform.rotation = spawnPoint.rotation;
                enemiesAvailable[_enemyType][i].SetActive(true);
                numberOfEnemiesAlive++;
                EnemiesAlive.Add(enemiesAvailable[_enemyType][i]);
                return;
            }
        }
    }

    public Transform[] GetWaypoints()
    {
        ///<summary>
        ///chooses randomly from an array of waypoints array fed to the GameMaster
        /// and returns the list of waypoints
        ///</summary>
        System.Random rng = new System.Random();
        int index = rng.Next(waypointHandler.Length);
        
        Transform[] waypoints = waypointHandler[index].GetComponent<Waypoints>().GetWaypoints();
        return waypoints;
    }
}
