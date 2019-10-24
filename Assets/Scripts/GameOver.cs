using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class GameOver : MonoBehaviour
{
    public Text WavesText;

    public SceneFader sceneFader;

    public TextMeshProUGUI coins;

    public string mainMenu = "MainMenu";

    private void OnEnable()
    {
        StartCoroutine(EnableGameOver());
    }

    IEnumerator EnableGameOver()
    {
        yield return new WaitForEndOfFrame();
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
