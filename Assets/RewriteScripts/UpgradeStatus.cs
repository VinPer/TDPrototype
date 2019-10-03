using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeStatus : MonoBehaviour
{
    public Text statusName;
    public Text statusValue;
    public Text priceText; 
    public void UpdateUpgradeStatus(int value)
    {
        statusValue.text =value.ToString();
    }
    public void UpdateUpgradeStatus(int value, int price)
    {
        statusValue.text = value.ToString();
        priceText.text = price.ToString();
    }
}
