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

    private Dictionary<string, GameObject> status;

    private void Start()
    {
        allTurrets = new List<TurretBlueprint>(TurretHandler.instance.allTurrets);
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
            allTurretsGUI.transform.GetChild(i).GetComponentsInChildren<Text>()[1].text = "$" + allTurrets[i].cost;
            allTurretsGUI.transform.GetChild(i)
                .GetComponentsInChildren<Text>()[0].text = allTurrets[i].name;
        }
        SelectTurret(0);
    }

    public void SelectTurret(int i)
    {
        turretSelected = allTurrets[i];
        turretImage.sprite = turretSelected.sprite;
        turretName.text = turretSelected.name;
        Dictionary<string, int> upgrades = UpgradeHandler.data.towerUpgrades[turretSelected.name];

        foreach (string item in status.Keys)
        {
            status[item].gameObject.SetActive(false);
        }
        foreach (string item in upgrades.Keys)
        {
            status[item].gameObject.SetActive(true);
            upgradeStatus = status[item].GetComponent<UpgradeStatus>();
            upgradeStatus.UpdateUpgradeStatus(upgrades[item]);
        }
    }

    public void UpgradeStatus(string _status)
    {
        Dictionary<string, int> upgrades = UpgradeHandler.data.towerUpgrades[turretSelected.name];

        if(upgrades[_status] < 3)
        {
            upgrades[_status]++;
            upgradeStatus = status[_status].GetComponent<UpgradeStatus>();
            upgradeStatus.UpdateUpgradeStatus(upgrades[_status]);
            Debug.Log(turretSelected.name + ": " + _status + ": " + upgrades[_status]);
            UpgradeHandler.instance.SaveData();
        }
    }

    public void BackToMenu()
    {
        sceneFader.FadeTo("MainMenu");
    }
}