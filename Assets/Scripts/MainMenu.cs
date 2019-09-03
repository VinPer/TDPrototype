using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string levelToLoad = "MainLevel";

    public SceneFader sceneFader;
    [SerializeField]
    private string nextSceneName;
    private int nextLevelIndex;
    public void Play()
    {
        sceneFader.FadeTo(levelToLoad);
    }

    public void Quit()
    {
        Debug.Log("Exiting");
        Application.Quit();
    }
}
