using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public SceneFader sceneFader;
    public void BuyUpgradeBlock1(string name)
    {
        UpgradeHandler.data.shopUpgrades["Block1"][name] = true;
    }
    public void BuyUpgradeBlock2(string name)
    {
        UpgradeHandler.data.shopUpgrades["Block2"][name] = true;
    }
    public void BuyUpgradeBlock3(string name)
    {
        UpgradeHandler.data.shopUpgrades["Block3"][name] = true;
    }
    public void BackToMenu()
    {
        sceneFader.FadeTo("MainMenu");
    }
}