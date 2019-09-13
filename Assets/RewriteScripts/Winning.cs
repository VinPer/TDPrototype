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
    public Text money;
    public Text lives;
    public Image[] stars;
    public Sprite starSprite;
    public Sprite emptySprite;

    public void Win()
    {
        if (string.Equals(nextLevel, ""))
        {
            nextLevelButton.interactable = false;
            nextLevelButtonText.text = "MORE LEVELS TO COME...";
            nextLevelButtonText.fontSize = 25;
        }
        money.text = "$" + PlayerStats.Money.ToString();
        lives.text = "♥" + PlayerStats.Lives.ToString();
        GetComponent<PlayerStats>().UpdateStars();
        for (int i = 0; i < PlayerStats.maxStars; i++)
        {
            if (i < PlayerStats.Stars) stars[i].sprite = starSprite;
            else stars[i].sprite = emptySprite;
        }
    }

    private void Toggle()
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
