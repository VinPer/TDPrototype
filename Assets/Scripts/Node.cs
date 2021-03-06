﻿using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class Node : MonoBehaviour
{
    private BuildManager buildManager;
    [HideInInspector]
    public GameObject turret;

    private TowerBase towerBuilded;
    [HideInInspector]
    public TurretBlueprint turretBlueprint;

    public Vector3 positionOffset;

    private Renderer rend;
    private Color startColor;
    public Color hoverColor;
    public Color cannotAffordColor;
    public Color turretSelectedColor;

    public Range range;
    GameObject buildEffect;
    GameObject sellEffect;
    GameObject upgradeEffect;

    private bool nodeOn;

    //private List<GameObject> turrets;

    private void Start()
    {
        nodeOn = false;

        buildManager = BuildManager.instance;

        rend = GetComponent<Renderer>();
        startColor = rend.material.color;

        range = Range.instance;
        //turrets = new List<GameObject>();
        //StartCoroutine(FillTurretList());

        buildEffect = Instantiate(buildManager.buildEffect, GetBuildPosition(), Quaternion.identity);
        sellEffect = Instantiate(buildManager.sellEffect, GetBuildPosition(), Quaternion.identity);
        upgradeEffect = Instantiate(buildManager.upgradeEffect, GetBuildPosition(), Quaternion.identity);
        buildEffect.transform.SetParent(transform);
        buildEffect.SetActive(false);
        sellEffect.transform.SetParent(transform);
        sellEffect.SetActive(false);
        upgradeEffect.transform.SetParent(transform);
        upgradeEffect.SetActive(false);
    }

    private void Update()
    {
        if (buildManager.GetTurretToBuild() != null && !nodeOn && PlayerStats.Money >= buildManager.GetTurretToBuild().cost && !turret)
        {
            nodeOn = !nodeOn;
            rend.material.color = turretSelectedColor;
        }
        if (buildManager.GetTurretToBuild() == null && nodeOn)
        {
            nodeOn = !nodeOn;
            rend.material.color = startColor;
        }
    }

    IEnumerator FillTurretList()
    {
        yield return new WaitForSeconds(.1f);
        foreach (TurretBlueprint obj in Shop.turretBlueprints)
        {
            GameObject newTurret = (GameObject)Instantiate(obj.prefab);
            newTurret.transform.position = transform.position;
            newTurret.transform.rotation = transform.rotation;
            newTurret.transform.SetParent(transform);
            newTurret.SetActive(false);
            //turrets.Add(newTurret);
        }
    }

    public Vector3 GetBuildPosition()
    {
        return transform.position + positionOffset;
    }

    private void OnMouseDown()
    {
        OnMouseDownExecute();
    }

    private void OnMouseDownExecute()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;
 
        if (turret != null)
        {
            buildManager.SelectNode(this);
            range.SetTarget(this);
            return;
        }

        if (!buildManager.CanBuild) return;

        // buildManager.BuildTurretOn(this);
        BuildTurret(buildManager.GetTurretToBuild());
    }

    public void CallOnMouseDown()
    {
        OnMouseDownExecute();
    }

    public void SelectNode()
    {
        if(turret != null)
        {
            buildManager.SelectNode(this);
            return;
        }

        if (!buildManager.CanBuild) return;

        BuildTurret(buildManager.GetTurretToBuild());
        //BuildTurret(buildManager.GetTurretIndexToBuild());
    }

    void BuildTurret(TurretBlueprint blueprint)
    {
        if (PlayerStats.Money < blueprint.cost)
        {
            Debug.Log("Not enough money to build that!");
            AudioManager.instance.Play("negate");
            return;
        }

        PlayerStats.Money -= blueprint.cost;

        turret = Instantiate(blueprint.prefab, GetBuildPosition(), Quaternion.identity);
        turret.transform.SetParent(transform);
        turret.SetActive(false);
        turret.SetActive(true);
        turret.name = blueprint.name;
        buildManager.DeselectTurret();


        if (turret.GetComponent<TowerBase>())
            towerBuilded = turret.GetComponent<TowerBase>();
        else
            towerBuilded = turret.GetComponentInChildren<TowerBase>();
        turretBlueprint = blueprint;

        //GameObject effect = Instantiate(buildManager.buildEffect, GetBuildPosition(), Quaternion.identity);
        //Destroy(effect, 5f);
        StartCoroutine(Effect.PlayEffect(buildEffect,transform));
        towerBuilded.upgrades = new Dictionary<string, int>(UpgradeHandler.data.towerUpgrades[blueprint.name]);
        Dictionary<string, int> backup = new Dictionary<string, int>(towerBuilded.upgrades);
        
        foreach (string item in backup.Keys)
        {
            towerBuilded.upgrades[item] = 0;
        }
        //Sound
        AudioManager.instance.Play("buildTurret");
        Debug.Log("Turret built!");
        PlayerStats.UpdateMoney();
    }

    //void BuildTurret(int index)
    //{
    //    if (PlayerStats.Money < Shop.turretBlueprints[index].cost)
    //    {
    //        Debug.Log("Not enough money to build that!");
    //        return;
    //    }

    //    PlayerStats.Money -= Shop.turretBlueprints[index].cost;

    //    turrets[index].SetActive(true);
    //    turret = turrets[index];
    //    turretBlueprint = Shop.turretBlueprints[index];
    //    if (turret.GetComponent<TowerBase>())
    //        tower = turret.GetComponent<TowerBase>();
    //    else
    //        tower = turret.GetComponentInChildren<TowerBase>();

    //    Debug.Log("Turret built!");
    //    PlayerStats.UpdateMoney();
    //}

    public void UpgradeTurret()
    {
        if (PlayerStats.Money < turretBlueprint.upgradeCost)
        {
            Debug.Log("Not enough money to upgrade that!");
            return;
        }

        PlayerStats.Money -= turretBlueprint.upgradeCost;

        //// Get rid of old turret
        //Destroy(turret);
        //// Build new turret
        //turret = Instantiate(turretBlueprint.upgradedPrefab, GetBuildPosition(), Quaternion.identity);
        //turret.transform.SetParent(transform);

        towerBuilded.UpgradeTower();

        //GameObject effect = Instantiate(buildManager.buildEffect, GetBuildPosition(), Quaternion.identity);
        //Destroy(effect, 5f);
        StartCoroutine(Effect.PlayEffect(upgradeEffect,transform));
        
        PlayerStats.UpdateMoney();
    }

    public void SellTurret()
    {
        // Add money at half the cost spent
        PlayerStats.Money += turretBlueprint.GetSellValue(towerBuilded.numberOfUpgrades);
        // Destroy turret and kill references
        turret.SetActive(false);
        turret = null;

        // Play effect
        //GameObject effect = Instantiate(buildManager.sellEffect, GetBuildPosition(), Quaternion.identity);
        //Destroy(effect, 5f);
        StartCoroutine(Effect.PlayEffect(sellEffect,transform));

        // Update money counter
        Debug.Log("Turret sold!");
        PlayerStats.UpdateMoney();
    }
    

    private void OnMouseEnter()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;
        if (!buildManager.CanBuild) return;

        if (PlayerStats.Money >= buildManager.GetTurretToBuild().cost)
        {
            rend.material.color = hoverColor;
            if(!turret)
                range.HoverTarget(this.gameObject);
        }
        else rend.material.color = cannotAffordColor;
    }

    private void OnMouseExit()
    {
        if (!nodeOn) rend.material.color = startColor;
        else rend.material.color = turretSelectedColor;
        if (TurretMenu.instance.isActive == false)
        {
            range.Hide();
        } 
    }
}
