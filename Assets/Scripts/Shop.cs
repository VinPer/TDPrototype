using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Shop : MonoBehaviour
{
    BuildManager buildManager;

    public static List<TurretBlueprint> turretBlueprints;
    public TurretBlueprint standardTurret;
    public TurretBlueprint missileLauncher;
    public TurretBlueprint bombSpawner;
    public TurretBlueprint flamethrower;
    public TurretBlueprint laserBeamer;
    
    private void Start()
    {
        buildManager = BuildManager.instance;
        

        // checks if TurretHandler has been carried from main menu, places the selected turrets
        if (TurretHandler.active)
        {
            turretBlueprints = SelectedTurrets.instance.selectedTurrets;
        }

        // if the scene is being played without going through the menu, load some defaults
        else
        {
            LoadDefault();
        }
        
        DrawTurrets();
    }

    // draw the turrets that are up for sale
    private void DrawTurrets()
    {
        Transform currentChild;

        // draw all from their data contained in TurretBlueprint
        for (int i = 0; i < turretBlueprints.Count; i++)
        {            
            currentChild = transform.GetChild(0).GetChild(i);
            currentChild.gameObject.SetActive(true);
            currentChild.GetComponent<Image>().sprite = turretBlueprints[i].sprite;
            currentChild.GetComponent<Image>().color = turretBlueprints[i].color;
            currentChild.GetComponentsInChildren<Text>()[1].text = "$" + turretBlueprints[i].cost;
            currentChild.GetComponentsInChildren<Text>()[0].text = turretBlueprints[i].name;
        }

        // draw the empty spots
        for (int i = turretBlueprints.Count; i < transform.childCount; i++)
        {
            currentChild = transform.GetChild(i);
            currentChild.GetComponent<Image>().color = new Color(0, 0, 0, 0);
            currentChild.GetComponentInChildren<Text>().text = "-";
        }
    }

    // instantiate the list and add defaults
    private void LoadDefault()
    {
        turretBlueprints = new List<TurretBlueprint>();
        turretBlueprints.Add(standardTurret);
        turretBlueprints.Add(missileLauncher);
        turretBlueprints.Add(bombSpawner);
        turretBlueprints.Add(laserBeamer);
        turretBlueprints.Add(flamethrower);
    }
    
    public void SelectTurret(int index)
    {
        if (index >= turretBlueprints.Count) return;
        buildManager.SelectTurretToBuild(turretBlueprints[index]);
        buildManager.SelectTurretIndexToBuild(index);
    }


    // leftover functions, here just in case

    public void SelectStandardTurret()
    {
        Debug.Log("Standard Turret Selected");
        buildManager.SelectTurretToBuild(standardTurret);
    }

    public void SelectMissileLauncher()
    {
        Debug.Log("Missile Launcher Selected");
        buildManager.SelectTurretToBuild(missileLauncher);
    }

    public void SelectLaserBeamer()
    {
        Debug.Log("Laser Beamer Selected");
        buildManager.SelectTurretToBuild(laserBeamer);
    }

    public void SelectBombSpawner()
    {
        Debug.Log("Bomb Spawner Selected");
        buildManager.SelectTurretToBuild(bombSpawner);
    }

    public void SelectFlamethrower()
    {
        Debug.Log("Flamethrower Selected");
        buildManager.SelectTurretToBuild(flamethrower);
    }
}
