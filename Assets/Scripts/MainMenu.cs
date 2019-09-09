using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string levelToLoad = "MainLevel";
    public string upgradeScene = "ShopTest";

    public SceneFader sceneFader;

    public void Play()
    {
        sceneFader.FadeTo(levelToLoad);
    }

    public void Quit()
    {
        Debug.Log("Exiting");
        Application.Quit();
    }
    public void Upgrades()
    {
        Debug.Log("Upgrades");
        sceneFader.FadeTo(upgradeScene);
    }
}
