﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Winning : MonoBehaviour
{
    public GameObject ui;
    public string nextLevel;
    public SceneFader sceneFader;
    public Button nextLevelButton;
    public Text nextLevelButtonText;
    public string menuScene = "MainMenu";
    
    public void Toggle()
    {
        ui.SetActive(!ui.activeSelf);

        if (ui.activeSelf)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }

    private void OnEnable()
    {
        if (string.Equals(nextLevel, ""))
        {
            nextLevelButton.interactable = false;
            nextLevelButtonText.text = "MORE LEVELS TO COME...";
            nextLevelButtonText.fontSize = 25;
        }
    }

    public void Continue()
    {
        Debug.Log("Going to next level");

        sceneFader.FadeTo(nextLevel);
    }

    public void Replay()
    {
        Toggle();
        sceneFader.FadeTo(SceneManager.GetActiveScene().name);
    }

    public void Menu()
    {
        Toggle();
        Debug.Log("Go to Menu");
        sceneFader.FadeTo(menuScene);
    }
}
