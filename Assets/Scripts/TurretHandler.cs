using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurretHandler : MonoBehaviour
{
    //public static TurretHandler instance;
    //// Keep the object to reference in the actual level so we can know which turrets are being used
    //private void Awake()
    //{
    //    DontDestroyOnLoad(gameObject);
    //    if (instance != null)
    //    {
    //        Destroy(instance.gameObject);
    //    }
    //    instance = this;
    //}

    // Lists for the turrets that are available and selected
    public List<TurretBlueprint> allTurrets;
    public List<string> allTurretsName;
    private List<TurretBlueprint> unlockedTurrets;
    public static List<string> selectedTurrets;
    public List<string> turretsToBuy = new List<string> {"Mortar", "Shotgun", "Gatling", "Buffer", "Tesla", "Flamethrower", "Freezer", "Spitter" };
    public List<string> turretsToUnlock = new List<string> { "Acid", "Radar", "Sniper", "Overheat", "Laser", "Charger" };
    public int max = 4;

    // uhh ignore this
    public List<TurretBlueprint> defaultSelected;

    // Elements that hold the turrets in the overlay
    public GameObject allTurretsGUI;
    public GameObject selectedTurretsGUI;

    public Button playButton;

    public static bool active = false;
    
    void Start()
    {
        active = true;
        //Upgrade max number of towers
        if (UpgradeHandler.data.shopUpgrades["Block1"]["MoreTowers"]) max = 5;
        if (UpgradeHandler.data.shopUpgrades["Block2"]["MoreTowersPlus"]) max = 6;

        // Instantiate the selected turrets list
        selectedTurrets = new List<string>();
        unlockedTurrets = new List<TurretBlueprint>();
        SelectedTurrets.allTurrets = new List<TurretBlueprint>(allTurrets);
        foreach (TurretBlueprint item in SelectedTurrets.instance.selectedTurrets)
        {
            selectedTurrets.Add(item.name);
        }
        foreach (TurretBlueprint item in SelectedTurrets.allTurrets)
        {
            allTurretsName.Add(item.name);
        }
        // Set the cost display for all available turrets
        for (int i = 0; i < allTurrets.Count; i++)
        {
            if(!string.Equals(allTurrets[i].name,"Bomb"))
            if (IsUnlock(allTurrets[i].name))
            {
                allTurretsGUI.transform.GetChild(i).GetComponentsInChildren<Text>()[1].text = "$" + allTurrets[i].cost;
                allTurretsGUI.transform.GetChild(i)
                    .GetComponentsInChildren<Text>()[0].text = allTurrets[i].name;
                allTurretsGUI.transform.GetChild(i).gameObject.SetActive(true);
                unlockedTurrets.Add(allTurrets[i]);
            }

        }

        // Add the first (max) turrets to the selected turrets list
        for (int i = 0; i < max; i++)
        {
            selectedTurretsGUI.transform.GetChild(i).gameObject.SetActive(true);
            //if (unlockedTurrets.Contains(allTurrets[i]))
            //{
            //   SelectedTurrets.instance.selectedTurrets.Add(allTurrets[i]);
            //}

        }

        UpdateSelectedTurrets();
    }

    public bool IsUnlock(string name)
    {
        bool res = false;
        if (turretsToBuy.Contains(name))
        {
            foreach (string item in UpgradeHandler.data.shopUpgrades.Keys)
            {
                if (UpgradeHandler.data.shopUpgrades[item].ContainsKey(name) && UpgradeHandler.data.shopUpgrades[item][name]) res = true;
            }
        }
        else if (turretsToUnlock.Contains(name))
        {
            foreach (string item in UpgradeHandler.data.levelsClear.Keys)
            {
                switch (name)
                {
                    case "Acid":
                        if (UpgradeHandler.data.levelsClear["2"] >= 0) res = true;
                        break;
                    case "Radar":
                        if (UpgradeHandler.data.levelsClear["3"] >= 0) res = true;
                        break;
                    case "Sniper":
                        if (UpgradeHandler.data.levelsClear["4"] >= 0) res = true;
                        break;
                    case "Overheat":
                        if (UpgradeHandler.data.levelsClear["5"] >= 0) res = true;
                        break;
                    case "Laser":
                        if (UpgradeHandler.data.levelsClear["6"] >= 0) res = true;
                        break;
                    case "Charger":
                        if (UpgradeHandler.data.levelsClear["7"] >= 0) res = true;
                        break;
                }
            }
        }
        else res = true;
        return res;
    }

    // Add a turret to the selected turrets list when it is clicked from the available list
    public void AssignTurret(int turretIndex)
    {
        TurretBlueprint turret = allTurrets[turretIndex];

        // Keeps the list limited to max turrets and without duplicates
        if (selectedTurrets.Count < max && !selectedTurrets.Contains(turret.name))
        {
            SelectedTurrets.instance.selectedTurrets.Add(turret);
            selectedTurrets.Add(turret.name);
        }
        UpdateSelectedTurrets();
    }

    // Remove a turret from the selected turrets list by clicking it
    public void RemoveTurret(int turretIndex)
    {
        // If you click an empty slot
        if (turretIndex >= selectedTurrets.Count) return;
        TurretBlueprint turret = SelectedTurrets.instance.selectedTurrets[turretIndex];
        //if (!selectedTurrets.Contains(turret)) return;
        
        //Transform element = selectedTurretsGUI.transform.GetChild(turretIndex);
        //element.GetComponent<Image>().color = new Color(0, 0, 0, 0);
        //element.GetComponentInChildren<Text>().text = "$0";

        SelectedTurrets.instance.selectedTurrets.Remove(turret);
        selectedTurrets.Remove(turret.name);
        UpdateSelectedTurrets();
    }

    // Updates the display of selected turrets
    private void UpdateSelectedTurrets()
    {
        Transform currentChildSelected;
        Transform currentChildAll;

        // For all turrets currently in the list, update image and cost
        for (int i = 0; i < selectedTurrets.Count; i++)
        {
            currentChildSelected = selectedTurretsGUI.transform.GetChild(i);
            currentChildAll = allTurretsGUI.transform.GetChild(
                allTurretsName.IndexOf(selectedTurrets[i]));
            currentChildSelected.GetComponent<Image>().sprite = currentChildAll.GetComponent<Image>().sprite;
            currentChildSelected.GetComponent<Image>().color = currentChildAll.GetComponent<Image>().color;
            currentChildSelected.GetComponentsInChildren<Text>()[1].text = "$" + SelectedTurrets.instance.selectedTurrets[i].cost;
            currentChildSelected.GetComponentsInChildren<Text>()[0].text = SelectedTurrets.instance.selectedTurrets[i].name;

            SelectedTurrets.instance.selectedTurrets[i].color = currentChildAll.GetComponent<Image>().color;
            SelectedTurrets.instance.selectedTurrets[i].sprite = currentChildAll.GetComponent<Image>().sprite;
            //Debug.Log(currentChildSelected.GetComponent<Button>().onClick.);
        }

        // For the empty slots in the list, hide the image and set cost to $0
        for (int i = selectedTurrets.Count; i < max; i++)
        {
            currentChildSelected = selectedTurretsGUI.transform.GetChild(i);
            currentChildSelected.GetComponent<Image>().color = new Color(0, 0, 0, 0);
            currentChildSelected.GetComponentsInChildren<Text>()[1].text = "$0";
            currentChildSelected.GetComponentsInChildren<Text>()[0].text = "";
        }
        if(selectedTurrets.Count < 1)
            playButton.interactable = false;
        else
            playButton.interactable = true;
    }
}
