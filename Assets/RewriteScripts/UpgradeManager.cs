using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager instance;
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one UpgradeManager in scene!");
            return;
        }

        instance = this;
    }

    List<TurretBlueprint> allTurrets;
    public GameObject allTurretsGUI;

    public SceneFader sceneFader;

    public TurretBlueprint turretSelected;
    private UpgradeStatus upgradeStatus;
    public Image turretImage;
    public Text turretName;
    public GameObject range;
    public GameObject fireRate;
    public GameObject damage;
    public GameObject piercing;
    public GameObject penetration;
    public GameObject ExplosionRadius;
    public GameObject debuffIntensity;
    public GameObject debuffDuration;
    public GameObject multiplier;
    public GameObject maxCharge;
    public GameObject chargeRate;
    public GameObject chainAMount;
    public GameObject buffRange;
    public GameObject buffDamage;
    public GameObject buffFireRate;
    public GameObject projectileAmount;
    public GameObject spreadReduction;
    public GameObject puddleDuration;
    public GameObject puddleSize;
    public GameObject chargeTime;

    public Text totalStars;
    public Text totalCoins;

    private List<string> activeStatus;

    private Dictionary<string, GameObject> status;
    public Dictionary<string, int> upgradesPrice = new Dictionary<string, int>
    {
        {"range", 100},
        {"fireRate",120 },
        {"damage", 120 },
        {"piercing",80 },
        {"penetration", 100 },
        {"explosionRadius", 150 },
        {"debuffIntensity", 150 },
        {"debuffDuration", 130 },
        {"multiplierSpeed", 120 },
        {"maxCharge", 150 },
        {"chargeRate", 120 },
        {"chainAmount", 110 },
        {"buffRange", 100 },
        {"buffDamage", 120 },
        {"buffFireRate", 130 },
        {"projectileAmount", 100 },
        {"spreadReduction", 100 },
        {"puddleDuration", 120 },
        {"puddleSize", 100 },
        {"chargeTime", 110 }
    };


    private void Start()
    {
        totalStars.text = UpgradeHandler.data.playerStats["TotalStars"].ToString();
        totalCoins.text = UpgradeHandler.data.playerStats["Coins"].ToString();
        allTurrets = new List<TurretBlueprint>(SelectedTurrets.allTurrets);
        status = new Dictionary<string, GameObject>
        {
            {"range", range},
            {"fireRate",fireRate },
            {"damage", damage },
            {"piercing",piercing },
            {"penetration", penetration },
            {"explosionRadius", ExplosionRadius },
            {"debuffIntensity", debuffIntensity },
            {"debuffDuration", debuffDuration },
            {"multiplierSpeed", multiplier },
            {"maxCharge", maxCharge },
            {"chargeRate", chargeRate },
            {"chainAmount", chainAMount },
            {"buffRange", buffRange },
            {"buffDamage", buffDamage },
            {"buffFireRate", buffFireRate },
            {"projectileAmount", projectileAmount },
            {"spreadReduction", spreadReduction },
            {"puddleDuration", puddleDuration },
            {"puddleSize", puddleSize },
            {"chargeTime", chargeTime }
        };
        for (int i = 0; i < allTurrets.Count; i++)
        {
            if(SelectedTurrets.instance.unlockedTurrets.Contains(allTurrets[i]))
            {
                allTurretsGUI.transform.GetChild(i).gameObject.SetActive(true);
            }
            else
            {
                allTurretsGUI.transform.GetChild(i).gameObject.SetActive(false);
            }

            
            allTurretsGUI.transform.GetChild(i).GetComponentsInChildren<Text>()[1].text = "$" + allTurrets[i].cost;
            allTurretsGUI.transform.GetChild(i)
                .GetComponentsInChildren<Text>()[0].text = allTurrets[i].name;
        }
        SelectTurret(0);
    }

    public void SelectTurret(int i)
    {
        activeStatus = new List<string>();
        turretSelected = allTurrets[i];
        turretImage.sprite = turretSelected.sprite;
        turretName.text = turretSelected.name;
        Dictionary<string, int> upgrades = UpgradeHandler.data.towerUpgrades[turretSelected.name];
        int coins = UpgradeHandler.data.playerStats["Coins"];
        foreach (string item in status.Keys)
        {
            status[item].gameObject.SetActive(false);
        }
        foreach (string item in upgrades.Keys)
        {
            status[item].gameObject.SetActive(true);
            activeStatus.Add(item);
            upgradeStatus = status[item].GetComponent<UpgradeStatus>();
            upgradeStatus.UpdateUpgradeStatus(upgrades[item],upgradesPrice[item]);
            if(upgradesPrice[item] > UpgradeHandler.data.playerStats["Coins"])
            {
                upgradeStatus.GetComponentInChildren<Button>().interactable = false;
            }
            else
            {

                upgradeStatus.GetComponentInChildren<Button>().interactable = true;
            }
        }
    }

    public void UpgradeStatus(string _status)
    {
        Dictionary<string, int> upgrades = UpgradeHandler.data.towerUpgrades[turretSelected.name];

        if(upgrades[_status] < 3 && upgradesPrice[_status] <= UpgradeHandler.data.playerStats["Coins"])
        {
            upgrades[_status]++;
            UpgradeHandler.data.playerStats["Coins"] -= upgradesPrice[_status];
            upgradeStatus = status[_status].GetComponent<UpgradeStatus>();
            upgradeStatus.UpdateUpgradeStatus(upgrades[_status]);
            Debug.Log(turretSelected.name + ": " + _status + ": " + upgrades[_status]);
            UpgradeHandler.instance.SaveData();
            foreach (string item in activeStatus)
            {
                if (upgradesPrice[item] > UpgradeHandler.data.playerStats["Coins"])
                {
                    status[item].GetComponentInChildren<Button>().interactable = false;
                }
                else
                {

                    status[item].GetComponentInChildren<Button>().interactable = true;
                }
            }
            totalStars.text = UpgradeHandler.data.playerStats["TotalStars"].ToString();
            totalCoins.text = UpgradeHandler.data.playerStats["Coins"].ToString();
        }
    }

    public void BackToMenu()
    {
        sceneFader.FadeTo("MainMenu");
    }
}