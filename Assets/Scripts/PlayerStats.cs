using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    public static int Money;
    public int startMoney = 1000;
    public static Text MoneyDisplay;
    public Text moneyDisplayStart;

    public static int Lives;
    public int startLives = 20;
    public static Text LivesDisplay;
    public Text livesDisplayStart;

    public static int WavesSurvived;
    public static int maxStars = 3;
    public static int Stars;
    public static int EnemiesKilled = 0;

    public static int Coins;

    private void Start()
    {
        Money = startMoney;
        MoneyDisplay = moneyDisplayStart;
        UpdateMoney();

        Lives = startLives;
        LivesDisplay = livesDisplayStart;
        UpdateLives();

        WavesSurvived = 0;
        Stars = 0;
        EnemiesKilled = 0;
    }

    public static void UpdateLives()
    {
        LivesDisplay.text = "♥" + Lives;
    }

    public static void UpdateMoney()
    {
        MoneyDisplay.text = "$" + Money;
    }

    public void UpdateStars()
    {
        if (GameManager.GameIsOver)
        {
            Stars++;
            if (Lives >= startLives) Stars++;
            if (Money >= startMoney / 2) Stars++;
            if (Stars > 3) Stars = 3;
        }
        else Stars = 0;
    }
}
