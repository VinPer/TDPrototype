using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RWaveSpawner : MonoBehaviour
{
    public GameManager gameManager;

    public enum SpawnerState { spawning, waiting, counting};

    public Transform spawnPoint;

    [System.Serializable]
    public class Wave
    {
        public string name;
        public EnemyWave[] enemies;
    }

    [System.Serializable]
    public class EnemyWave
    {
        public Transform enemy;
        public int count = 0;
        public int rate = 0;
    }

    public Wave[] waves;
    private int nextWave = 0;
    public float timeBetweenWaves = 5f;
    private float waveCountdown;
    
    public Text wavesLeft;
    public Text wavesLeftGO;

    private float searchCountdown = 1f;
    private SpawnerState state = SpawnerState.counting;

    public Transform waypointHandler;

    public static int EnemiesAlive;
    public bool countingDown = true;
    public Text waveCountdownText;
    public Text waveNumber;

    private void Start()
    {
        waveCountdown = timeBetweenWaves;
        WavesLeft();

        EnemiesAlive = 0;
    }

    private void Update()
    {

        if(state == SpawnerState.waiting)
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

        if(waveCountdown <= 0)
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
            wavesLeftGO.color = Color.red;
            wavesLeftGO.text = "It was the last wave...";
        }
        else
        {
            wavesLeft.text = (waves.Length - nextWave).ToString();
            wavesLeftGO.text = wavesLeft.text;
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

    IEnumerator SpawnWave(Wave _wave)
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
            if(rate == 0)
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

    public void SpawnEnemy(Transform _enemy)
    {
        Transform e = Instantiate(_enemy, spawnPoint.position, spawnPoint.rotation);
        e.SetParent(transform);
    }

    public Transform[] GetWaypoints()
    {
        Transform[] waypoints = waypointHandler.GetComponent<Waypoints>().GetWaypoints();
        return waypoints;
    }
}
