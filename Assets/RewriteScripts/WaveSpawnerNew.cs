using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveSpawnerNew : MonoBehaviour
{
    [System.Serializable]
    public class Waves
    {
        public WaveBurst[] burst;
    }

    public static int EnemiesAlive = 0;

    private float searchCountdown = 1f;
    public enum SpawnerState { spawning, waiting, counting };
    private SpawnerState state = SpawnerState.counting;

    public Waves[] waves;
    private Dictionary<Enums.EnemyType, int> enemiesCount;
    private Dictionary<Enums.EnemyType, GameObject> enemiesType;

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


    [Header("Other")]
    public Transform spawnPoint;
    public Transform waypointHandler;
    public bool infiniteOffset = true;

    public float timeBetweenWaves = 5f;

    public Text waveCountdownText;
    public Text wavesLeft;

    private float waveCountdown = 2f;
    private int nextWave = 0;
    public bool countingDown = true;

    // Start is called before the first frame update
    void Start()
    {
        //pool
        PoolEnemies();
    }

    void PoolEnemies()
    {
        enemiesCount = new Dictionary<Enums.EnemyType, int>();
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
            for (int i = 0; i < enemiesCount[enemyType]; i++)
            {
                GameObject obj = (GameObject)Instantiate(enemiesType[enemyType], spawnPoint.position, spawnPoint.rotation);
                obj.transform.SetParent(transform);
                obj.SetActive(false);
            }
        }
    }

    private void Update()
    {

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
        else if (countingDown)
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
        if (nextWave + 1 > waves.Length - 1)
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
        if (nextWave + 1 > waves.Length - 1)
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
        searchCountdown -= Time.deltaTime;
        if (searchCountdown <= 0f)
        {
            searchCountdown = 1f;
            if (GameObject.FindGameObjectWithTag("Enemy") == null)
            {
                return false;
            }
        }
        return true;
    }

    IEnumerator SpawnWave(Waves _wave)
    {
        state = SpawnerState.spawning;
        int count;
        int rate;
        foreach (EnemyWave enemy in _wave.enemies)
        {
            count = enemy.count;
            if (count == 0)
                count = Random.Range(1, 10);
            rate = enemy.rate;
            if (rate == 0)
                rate = Random.Range(1, 5);
            for (int i = 0; i < count; i++)
            {
                SpawnEnemy(enemy.enemy);
                yield return new WaitForSeconds(1f / rate);
            }
        }

        state = SpawnerState.waiting;

        yield break;
    }

    public Transform[] GetWaypoints()
    {
        Transform[] waypoints = waypointHandler.GetComponent<Waypoints>().GetWaypoints();
        return waypoints;
    }
}
