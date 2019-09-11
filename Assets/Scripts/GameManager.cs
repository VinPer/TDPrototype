using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [HideInInspector]
    public static bool GameIsOver;

    public string levelNumber;

    public GameObject gameOverUI;
    public GameObject debugUI;
    public GameObject turretUI;
    public GameObject skillUI;
    public GameObject winUI;

    public Text switchText;

    public static GameManager instance;
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one BuildManager in scene!");
            return;
        }

        instance = this;
    }
    private void Start()
    {
        GameIsOver = false;
        AudioManager.instance.Play(SceneManager.GetActiveScene().name);
        Debug.Log(SceneManager.GetActiveScene().name);

        if (turretUI.activeSelf) switchText.text = "Skills";
        else switchText.text = "Turrets";
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L)) ToggleDebug();
        if (GameIsOver) return;
        if (PlayerStats.Lives <= 0) EndGame();
    }

    private void ToggleDebug()
    {
        debugUI.SetActive(!debugUI.activeSelf);
    }

    public void SwitchMenus()
    {
        turretUI.SetActive(!turretUI.activeSelf);
        skillUI.SetActive(!skillUI.activeSelf);
        if (turretUI.activeSelf) switchText.text = "Skills";
        else switchText.text = "Turrets";
    }

    private void EndGame()
    {
        GameIsOver = true;
        UpgradeHandler.instance.SaveData();
        gameOverUI.SetActive(true);
        AudioManager.instance.Play("Defeat");
        AudioManager.instance.Stop(SceneManager.GetActiveScene().name);
    }

    public void Winning()
    {
        GameIsOver = true;
        GetComponent<Winning>().Win();
        UpgradeHandler.data.levelsClear[levelNumber] = PlayerStats.Stars;
        UpgradeHandler.instance.SaveData();
        winUI.SetActive(true);
        AudioManager.instance.Play("Victory");
        AudioManager.instance.Stop(SceneManager.GetActiveScene().name);
    }
}
