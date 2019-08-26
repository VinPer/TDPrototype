﻿using UnityEngine;
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
    public TurretMenu turretMenu;

    private TurretBlueprint turretToBuild;
    private Node selectedNode;

    private int turretIndexToBuild;

    public static List<GameObject> TurretsBuilded;

    public bool CanBuild { get { return turretToBuild != null; } }

    private void Start()
    {
        TurretsBuilded = new List<GameObject>();
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
        turretToBuild = null;
        turretIndexToBuild = -1;
        turretMenu.SetTarget(node);
        //nodeUI.SetTarget(node);
    }

    public void DeselectNode()
    {
        selectedNode = null;
        //nodeUI.Hide();
        turretMenu.CloseMenu();
        Range.instance.Hide();
        
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
