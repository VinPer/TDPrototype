using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string levelToLoad = "MainLevel";
    public string upgradeScene = "ShopTest";
    public string shopScene = "ShopUpgrades";

    public SceneFader sceneFader;

    private void Start()
    {
        //UpgradeHandler.instance.SaveData();
    }


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
    public void Shop()
    {
        Debug.Log("Upgrades");
        sceneFader.FadeTo(shopScene);
    }
    public void ResetSave()
    {
        UpgradeHandler.data.Reset();
        UpgradeHandler.instance.SaveData();
    }
}
