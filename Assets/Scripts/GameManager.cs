
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [HideInInspector]
    public static bool GameIsOver;

    public GameObject gameOverUI;
    public GameObject debugUI;

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
        debugUI.SetActive(!debugUI.activeSelf);
    }

    private void EndGame()
    {
        GameIsOver = true;
        gameOverUI.SetActive(true);
    }
}
