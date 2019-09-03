using UnityEngine;
using System.Collections.Generic;

public class BuildManager : MonoBehaviour
{
    public static BuildManager instance;
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one BuildManager in scene!");
            return;
        }

        instance = this;
    }
    public GameObject buildEffect;
    public GameObject sellEffect;
    private TurretMenu turretMenu;

    private TurretBlueprint turretToBuild;
    private Node selectedNode;

    private int turretIndexToBuild;

    public static List<GameObject> TurretsBuilded;

    public bool CanBuild { get { return turretToBuild != null; } }

    private void Start()
    {
        TurretsBuilded = new List<GameObject>();
        turretMenu = TurretMenu.instance;
    }

    public void SelectTurretToBuild(TurretBlueprint turret)
    {
        turretToBuild = turret;
        DeselectNode();
    }

    public void SelectTurretIndexToBuild(int index)
    {
        turretIndexToBuild = index;
        DeselectNode();
    }

    public void SelectNode(Node node)
    {
        if (selectedNode == node)
        {
            DeselectNode();
            return;
        }
        selectedNode = node;
        DeselectTurret();
        turretMenu.SetTarget(node);
        //nodeUI.SetTarget(node);
    }

    public void DeselectNode()
    {
        selectedNode = null;
        //nodeUI.Hide();
        turretMenu.CloseMenu();
    }

    public void DeselectTurret()
    {
        turretIndexToBuild = -1;
        turretToBuild = null;
    }

    public TurretBlueprint GetTurretToBuild()
    {
        return turretToBuild;
    }

    public int GetTurretIndexToBuild()
    {
        return turretIndexToBuild;
    }

    public void Sell()
    {
        selectedNode.SellTurret();
        BuildManager.instance.DeselectNode();
    }

    public void Upgrade()
    {
        selectedNode.UpgradeTurret();
        BuildManager.instance.DeselectNode();
    }

}
