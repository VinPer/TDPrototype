using UnityEngine;
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

    public void Continue()
    {
        Debug.Log("Going to next level");
        if (nextLevel == "")
        {
            nextLevelButton.interactable = false;
            nextLevelButtonText.text = "MORE LEVELS TO COME...";
        }
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
