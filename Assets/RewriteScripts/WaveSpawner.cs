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
    private Dictionary<Enums.EnemyType, int> enemiesCount; //Count of each type of enemy
    private Dictionary<Enums.EnemyType, GameObject> enemiesType; //Which GameObject is related to each type
    private Dictionary<Enums.EnemyType, List<GameObject>> enemiesAvailable; //Enemies used in level

    public static List<GameObject> EnemiesAlive;

    public GameObject basicEnemy;
    public GameObject fastEnemy;
    public GameObject tankEnemy;
    public GameObject shildedEnemy;
    public GameObject cluster;
    public GameObject fireElement;
    public GameObject acidElement;
    public GameObject iceElement;
    public GameObject flying;
    public GameObject zip;
    public GameObject invisible;


    [Header("Other")]
    public Transform spawnPoint;
    public Transform waypointHandler;
    public bool infiniteOffset = true;

    public float timeBetweenWaves = 5f;

    public Text waveCountdownText;
    public Text wavesLeft;
    public Text waveNumber;

    private float waveCountdown = 2f;
    private int nextWave = 0;

    private bool stopWaveSpawner = false;
    
    // Start is called before the first frame update
    void Start()
    {
        EnemiesAlive = new List<GameObject>();

        //pool
        PoolEnemies();
        waveCountdown = timeBetweenWaves;
        state = SpawnerState.counting;
        WavesLeft();
        stopWaveSpawner = false;
    }

    void PoolEnemies()
    {
        enemiesCount = new Dictionary<Enums.EnemyType, int>();
        enemiesAvailable = new Dictionary<Enums.EnemyType, List<GameObject>>();
        enemiesType = new Dictionary<Enums.EnemyType, GameObject>
        {
            { Enums.EnemyType.basic , basicEnemy},
            { Enums.EnemyType.fast , fastEnemy },
            { Enums.EnemyType.tank , tankEnemy },
            { Enums.EnemyType.shilded , shildedEnemy },
            { Enums.EnemyType.cluster , cluster },
            { Enums.EnemyType.fireElement , fireElement },
            { Enums.EnemyType.acidElement , acidElement },
            { Enums.EnemyType.iceElement , iceElement },
            { Enums.EnemyType.flying , flying },
            { Enums.EnemyType.zip , zip },
            { Enums.EnemyType.invisible , invisible }
        };

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
        foreach (Enums.EnemyType enemyType in enemiesCount.Keys)
        {
            int n = 0;
            for (int i = 0; i < enemiesCount[enemyType]; i++)
            {
                GameObject obj = (GameObject)Instantiate(enemiesType[enemyType], spawnPoint.position, spawnPoint.rotation);
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
        waveNumber.text = "Enemies Alive: " + numberOfEnemiesAlive;

        if (stopWaveSpawner) return;
        
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
            waveCountdownText.text = string.Format("{0:00.00}", waveCountdown);
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
                //gameManager.Winning();
            }
        }
        else
        {
            state = SpawnerState.counting;
            waveCountdown = timeBetweenWaves;
            nextWave++;
            WavesLeft();

        }

    }

    void WavesLeft()
    {
        if (nextWave + 1 > waves.Length)
        {
            wavesLeft.text = "Last wave!";
        }
        else
        {
            wavesLeft.text = (waves.Length - nextWave).ToString();
        }
    }

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

    void SpawnEnemy(Enums.EnemyType _enemyType)
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
        Transform[] waypoints = waypointHandler.GetComponent<Waypoints>().GetWaypoints();
        return waypoints;
    }
}
