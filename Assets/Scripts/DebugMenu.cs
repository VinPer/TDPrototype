using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugMenu : MonoBehaviour
{
    public GameObject[] enemies;
    private Dropdown enemy;
    private InputField input;

    public GameObject GameMaster;
    private WaveSpawner waveSpawner;

    private bool speedUp = false;
    public float speedUpMultiplier = 4f;

    public float delay = .2f;

    public Sprite normalSpeed;
    public Sprite fastSpeed;

    private void Start()
    {
        enemy = transform.GetComponentInChildren<Dropdown>();
        input = transform.GetComponentInChildren<InputField>();
        waveSpawner = GameMaster.transform.GetComponent<WaveSpawner>();
    }

    private void OnEnable()
    {
        NormalSpeed();
    }

    public void SpawnEnemy()
    {
        int amount;
        try
        {
            amount = Int32.Parse(input.text);
        }
        catch
        {
            amount = 0;
        }

        if (amount < 1 || amount > 1000) return;

        StartCoroutine(SpawnCoroutine(amount));
    }

    private IEnumerator SpawnCoroutine(int amount)
    {
        for(int i = 0; i < amount; i++)
        {
            // waveSpawner.SpawnEnemy(enemies[enemy.value].transform);
            yield return new WaitForSeconds(delay);
        }
    }

    public void ToggleSpeed(Button speedButton)
    {
        speedUp = (!speedUp);

        if (speedUp)
        {
            Time.timeScale = speedUpMultiplier;
            speedButton.image.sprite = fastSpeed;
        }
        else
        {
            Time.timeScale = 1f;
            speedButton.image.sprite = normalSpeed;
        }
    }
    public void NormalSpeed()
    {
        speedUp = false;
        Time.timeScale = 1f;
    }

    //public void ToggleWave()
    //{
    //    waveSpawner.countingDown = !waveSpawner.countingDown;
    //}
}
