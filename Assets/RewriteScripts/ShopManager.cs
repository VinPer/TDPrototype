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
        public string name;
        public Transform transform;
        public int starsRequired;
    }
    public Dictionary<string, Dictionary<string, int>> upgradesPrice = new Dictionary<string, Dictionary<string, int>>
        {
            { "Block1", new Dictionary<string, int>
            {
                { "MoreTowers", 100 },
                { "MoreSkills", 100 },
                { "ElementalBlast", 150},
                { "Mortar", 150 },
                { "Shotgun", 120 },
                { "Gatling", 120 }
            } },
            { "Block2", new Dictionary<string, int>
            {
                { "MoreTowersPlus", 200 },
                { "MoreSkillsPlus", 200 },
                { "BuffAllTowers", 180 },
                { "Buffer", 150 },
                { "Tesla", 300 }
            } },
            { "Block3", new Dictionary<string, int>
            {
                { "Flamethrower", 400 },
                { "Freezer", 350 },
                { "Spitter", 320 },
                { "UltimateTower", 350 },
                { "Nuke", 400 }
            } },
        };
    private void Start()
    {
        totalStars.text = UpgradeHandler.data.playerStats["TotalStars"].ToString();
        unspentStars.text = UpgradeHandler.data.playerStats["Coins"].ToString();
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
                    if(!UpgradeHandler.data.shopUpgrades[block.name][item.name])
                        item.interactable = true;
                    else
                    {
                        item.interactable = false;
                        item.GetComponentInChildren<Text>().text += " Unlocked"; 
                    }
                }
            }
        }
    }

    public void BuyUpgradeBlock1(string name)
    {
        string block = "Block1";
        if (UpgradeHandler.data.playerStats["Coins"] >= upgradesPrice[block][name])
        {
            UpgradeHandler.data.shopUpgrades["Block1"][name] = true;
            UpgradeHandler.data.playerStats["Coins"] -= upgradesPrice[block][name];
            totalStars.text = UpgradeHandler.data.playerStats["TotalStars"].ToString();
            unspentStars.text = UpgradeHandler.data.playerStats["Coins"].ToString();
            GameObject.Find(name).GetComponent<Button>().interactable = false;
            UpgradeHandler.instance.SaveData();
        }
    }
    public void BuyUpgradeBlock2(string name)
    {
        string block = "Block2";
        if (UpgradeHandler.data.playerStats["Coins"] >= upgradesPrice[block][name])
        {
            UpgradeHandler.data.shopUpgrades["Block2"][name] = true;
            UpgradeHandler.data.playerStats["Coins"] -= upgradesPrice[block][name];
            totalStars.text = UpgradeHandler.data.playerStats["TotalStars"].ToString();
            unspentStars.text = UpgradeHandler.data.playerStats["Coins"].ToString();
            UpgradeHandler.instance.SaveData();
        }
    }
    public void BuyUpgradeBlock3(string name)
    {
        string block = "Block3";
        if (UpgradeHandler.data.playerStats["Coins"] >= upgradesPrice[block][name])
        {
            UpgradeHandler.data.shopUpgrades[block][name] = true;
            UpgradeHandler.data.playerStats["Coins"] -= upgradesPrice[block][name];
            totalStars.text = UpgradeHandler.data.playerStats["TotalStars"].ToString();
            unspentStars.text = UpgradeHandler.data.playerStats["Coins"].ToString();
            UpgradeHandler.instance.SaveData();
        }
    }

    public void BackToMenu()
    {
        sceneFader.FadeTo("MainMenu");
    }
}