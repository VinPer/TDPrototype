using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

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
    public GameObject unlockTowerCanvas;

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
        if (UpgradeHandler.data.levelsClear[GameManager.instance.levelNumber] == 0)
        {
            int level = int.Parse(GameManager.instance.levelNumber);
            level++;
            UpgradeHandler.data.levelsClear[level.ToString()] = 0;
            print(level.ToString() + " : " + UpgradeHandler.data.levelsClear[level.ToString()]);
        }
        if (UpgradeHandler.data.levelsClear[GameManager.instance.levelNumber] < PlayerStats.Stars)
        {
            UpgradeHandler.data.playerStats["TotalStars"] += PlayerStats.Stars - UpgradeHandler.data.levelsClear[GameManager.instance.levelNumber];
            UpgradeHandler.data.playerStats["UnspentStars"] += PlayerStats.Stars - UpgradeHandler.data.playerStats["UnspentStars"];
            UpgradeHandler.data.levelsClear[GameManager.instance.levelNumber] = PlayerStats.Stars;
        }
        UnlockTower();
    }

    private void UnlockTower()
    {
        Dictionary<string, string> towersToUnlock = new Dictionary<string, string>
        {
            {"1", "Acid" },
            {"2", "Radar" },
            {"3", "Sniper" },
            {"4", "Overheat" },
            {"5", "Laser" },
            {"6", "Charger" },
        };
        if (UpgradeHandler.data.unlockedTowers.ContainsKey(towersToUnlock[GameManager.instance.levelNumber]))
        {
            if (!UpgradeHandler.data.unlockedTowers[towersToUnlock[GameManager.instance.levelNumber]])
            {
                unlockTowerCanvas.SetActive(true);
                unlockTowerCanvas.GetComponent<UnlockTowerCanvas>().turretUnlocked = towersToUnlock[GameManager.instance.levelNumber];


                UpgradeHandler.data.unlockedTowers[towersToUnlock[GameManager.instance.levelNumber]] = true;
            }
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
