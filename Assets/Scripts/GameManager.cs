﻿using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [HideInInspector]
    public static bool GameIsOver;

    public GameObject gameOverUI;
    public GameObject debugUI;
    public GameObject turretUI;
    public GameObject skillUI;
    public GameObject winUI;

    public static GameManager instance;
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one BuildManager in scene!");
            return;
        }

        instance = this;
    }
    private void Start()
    {
        GameIsOver = false;
        AudioManager.instance.Play(SceneManager.GetActiveScene().name);
        Debug.Log(SceneManager.GetActiveScene().name);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L)) ToggleDebug();
        if (GameIsOver) return;
        if (PlayerStats.Lives <= 0) EndGame();
    }

    private void ToggleDebug()
    {
        debugUI.SetActive(!debugUI.activeSelf);
    }

    public void SwitchMenus()
    {
        turretUI.SetActive(!turretUI.activeSelf);
        skillUI.SetActive(!skillUI.activeSelf);
    }

    private void EndGame()
    {
        GameIsOver = true;
        gameOverUI.SetActive(true);
        AudioManager.instance.Play("Defeat");
        AudioManager.instance.Stop(SceneManager.GetActiveScene().name);
    }

    public void Winning()
    {
        GameIsOver = true;
        winUI.SetActive(true);
        AudioManager.instance.Play("Victory");
        AudioManager.instance.Stop(SceneManager.GetActiveScene().name);
    }
}
