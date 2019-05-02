
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [HideInInspector]
    public static bool GameIsOver;

    public GameObject gameOverUI;

    public float speedUpMultiplier = 5f;
    private bool speedUp = false;

    private void Start()
    {
        GameIsOver = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L)) Toggle();
        if (GameIsOver) return;
        if (PlayerStats.Lives <= 0) EndGame();
    }

    private void Toggle()
    {
        speedUp = (!speedUp);

        if (speedUp)
        {
            Time.timeScale = speedUpMultiplier;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }

    private void EndGame()
    {
        GameIsOver = true;
        gameOverUI.SetActive(true);
    }
}
