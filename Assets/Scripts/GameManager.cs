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

    [Header("Coin System")]
    public int enemiesKilledBonus = 10;
    public int moneyBonus = 1;
    [Range(0,100)]
    public int levelBonus = 10;
    private bool win = false;

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
        win = false;
        ReciveCoins();
    }

    public void Winning()
    {
        GameIsOver = true;
        GetComponent<Winning>().Win();
        win = true;
        ReciveCoins();
    }

    public void ReciveCoins() {
        //COIN SYSTEM
        int coins = PlayerStats.EnemiesKilled * enemiesKilledBonus + PlayerStats.Money * moneyBonus;
        coins += (int)Mathf.Round((coins * levelBonus)/100);
        if (win) coins *= PlayerStats.Stars;
        UpgradeHandler.data.playerStats["Coins"] += coins;
        print("Coins: " + coins);
        print("Total coins: " + UpgradeHandler.data.playerStats["Coins"]);

        UpgradeHandler.instance.SaveData();
        GameManager.instance.winUI.SetActive(true);
        AudioManager.instance.Play("Victory");
        AudioManager.instance.Stop(SceneManager.GetActiveScene().name);
    }
}
