﻿
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [HideInInspector]
    public static bool GameIsOver;

    public GameObject gameOverUI;
    public GameObject debugUI;
    public GameObject turretUI;
    public GameObject skillUI;

    private void Start()
    {
        GameIsOver = false;
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
    }
}
