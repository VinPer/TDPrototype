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
                { "MoreTowers", 1 },
                { "MoreSkills", 1 },
                { "ElementalBlast", 1 },
                { "Mortar", 1 },
                { "Shotgun", 1 },
                { "Gatling", 1 }
            } },
            { "Block2", new Dictionary<string, int>
            {
                { "MoreTowersPlus", 1 },
                { "MoreSkillsPlus", 1 },
                { "BuffAllTowers", 1 },
                { "Buffer", 1 },
                { "Tesla", 1 }
            } },
            { "Block3", new Dictionary<string, int>
            {
                { "Flamethrower", 1 },
                { "Freezer", 1 },
                { "Spitter", 1 },
                { "UltimateTower", 1 },
                { "Nuke", 1 }
            } },
        };
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
        if (UpgradeHandler.data.playerStats["UnspentStars"] >= upgradesPrice["Block1"][name])
        {
            UpgradeHandler.data.shopUpgrades["Block1"][name] = true;
            UpgradeHandler.data.playerStats["UnspentStars"] -= upgradesPrice["Block1"][name];
            totalStars.text = UpgradeHandler.data.playerStats["TotalStars"].ToString();
            unspentStars.text = UpgradeHandler.data.playerStats["UnspentStars"].ToString();
            GameObject.Find(name).GetComponent<Button>().interactable = false;
            UpgradeHandler.instance.SaveData();
        }
    }
    public void BuyUpgradeBlock2(string name)
    {
        if (UpgradeHandler.data.playerStats["UnspentStars"] >= upgradesPrice["Block2"][name])
        {
            UpgradeHandler.data.shopUpgrades["Block2"][name] = true;
            UpgradeHandler.data.playerStats["UnspentStars"] -= upgradesPrice["Block2"][name];
            totalStars.text = UpgradeHandler.data.playerStats["TotalStars"].ToString();
            unspentStars.text = UpgradeHandler.data.playerStats["UnspentStars"].ToString();
            UpgradeHandler.instance.SaveData();
        }
    }
    public void BuyUpgradeBlock3(string name)
    {
        if (UpgradeHandler.data.playerStats["UnspentStars"] >= upgradesPrice["Block3"][name])
        {
            UpgradeHandler.data.shopUpgrades["Block3"][name] = true;
            UpgradeHandler.data.playerStats["UnspentStars"] -= upgradesPrice["Block3"][name];
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