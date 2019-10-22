using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOver : MonoBehaviour
{
    public Text WavesText;

    public SceneFader sceneFader;

    public TextMeshProUGUI coins;

    public string mainMenu = "MainMenu";

    private void OnEnable()
    {
        WavesText.text = PlayerStats.WavesSurvived + "";
        coins.text = "Coins: " + PlayerStats.Coins;
    }

    public void Retry()
    {
        sceneFader.FadeTo(SceneManager.GetActiveScene().name);
    }

    public void Menu()
    {
        sceneFader.FadeTo(mainMenu);
    }

}
