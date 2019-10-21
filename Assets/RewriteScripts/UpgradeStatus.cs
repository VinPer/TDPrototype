using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeStatus : MonoBehaviour
{
    public Text statusName;
    public Image[] statusValue;
    public Sprite unlocked;
    public Sprite locked;

    public Text priceText; 
    public void UpdateUpgradeStatus(int value)
    {
        for (int i = 0; i < statusValue.Length; i++)
        {
            print(value);
            print(i);
            if (i < value) statusValue[i].sprite = unlocked;
            else statusValue[i].sprite = locked;
        }
    }
    public void UpdateUpgradeStatus(int value, int price)
    {
        for (int i = 0; i < statusValue.Length; i++)
        {
            if (i < value) statusValue[i].sprite = unlocked;
            else statusValue[i].sprite = locked;
        }
        priceText.text = price.ToString();
    }
}
