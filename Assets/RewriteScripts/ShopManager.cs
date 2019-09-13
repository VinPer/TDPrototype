using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public SceneFader sceneFader;

    public Block[] blocks;

    public Text totalStars;
    public Text unspentStars;

    [System.Serializable]
    public class Block
    {
        public Transform transform;
        public int starsRequired;
    }

    private void Start()
    {
        totalStars.text = UpgradeHandler.data.playerStats["TotalStars"].ToString();
        unspentStars.text = UpgradeHandler.data.playerStats["UnspentStars"].ToString();
        foreach (Block block in blocks)
        {
            if (UpgradeHandler.data.playerStats["TotalStars"] < block.starsRequired)
            {
                foreach (Button item in block.transform.GetComponentsInChildren<Button>())
                {
                    item.interactable = false;
                }
            }
            else
            {
                foreach (Button item in block.transform.GetComponentsInChildren<Button>())
                {
                    item.interactable = true;
                }
            }
        }
    }

    public void BuyUpgradeBlock1(string name)
    {
        if(UpgradeHandler.data.playerStats["UnspentStars"] > 0)
        {
            UpgradeHandler.data.shopUpgrades["Block1"][name] = true;
            UpgradeHandler.data.playerStats["UnspentStars"] -= 1;
            totalStars.text = UpgradeHandler.data.playerStats["TotalStars"].ToString();
            unspentStars.text = UpgradeHandler.data.playerStats["UnspentStars"].ToString();
            UpgradeHandler.instance.SaveData();
        }
    }
    public void BuyUpgradeBlock2(string name)
    {
        if (UpgradeHandler.data.playerStats["UnspentStars"] > 0)
        {
            UpgradeHandler.data.shopUpgrades["Block2"][name] = true;
            UpgradeHandler.data.playerStats["UnspentStars"] -= 1;
            totalStars.text = UpgradeHandler.data.playerStats["TotalStars"].ToString();
            unspentStars.text = UpgradeHandler.data.playerStats["UnspentStars"].ToString();
            UpgradeHandler.instance.SaveData();
        }
    }
    public void BuyUpgradeBlock3(string name)
    {
        if (UpgradeHandler.data.playerStats["UnspentStars"] > 0)
        {
            UpgradeHandler.data.shopUpgrades["Block3"][name] = true;
            UpgradeHandler.data.playerStats["UnspentStars"] -= 1;
            totalStars.text = UpgradeHandler.data.playerStats["TotalStars"].ToString();
            unspentStars.text = UpgradeHandler.data.playerStats["UnspentStars"].ToString();
            UpgradeHandler.instance.SaveData();
        }
    }
    public void BackToMenu()
    {
        sceneFader.FadeTo("MainMenu");
    }
}