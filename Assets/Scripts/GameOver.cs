using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public Text WavesText;

    public SceneFader sceneFader;

    public string mainMenu = "MainMenu";

    private void OnEnable()
    {
        WavesText.text = PlayerStats.WavesSurvived + "";
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
