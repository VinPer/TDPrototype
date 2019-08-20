using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string levelToLoad = "MainLevel";

    public SceneFader sceneFader;

    private void Awake()
    {
        AudioManager.instance.Play("Menu");
    }

    public void Play()
    {
        sceneFader.FadeTo(levelToLoad);
        AudioManager.instance.Stop("Menu");
    }

    public void Quit()
    {
        Debug.Log("Exiting");
        Application.Quit();
    }
}
