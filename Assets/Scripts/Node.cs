using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class Node : MonoBehaviour
{
    public BuildManager buildManager;
    [HideInInspector]
    public GameObject turret;

    private TowerBase tower;
    [HideInInspector]
    public TurretBlueprint turretBlueprint;

    public Vector3 positionOffset;

    private Renderer rend;
    private Color startColor;
    public Color hoverColor;
    public Color cannotAffordColor;
    
    public Range range;

    //private List<GameObject> turrets;

    private void Start()
    {
        buildManager = BuildManager.instance;

        rend = GetComponent<Renderer>();
        startColor = rend.material.color;
        
        range = FindObjectOfType<Range>();
        //turrets = new List<GameObject>();
        //StartCoroutine(FillTurretList());
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


        if (turret.GetComponent<TowerBase>())
            tower = turret.GetComponent<TowerBase>();
        else
            tower = turret.GetComponentInChildren<TowerBase>();
        turretBlueprint = blueprint;

        GameObject effect = Instantiate(buildManager.buildEffect, GetBuildPosition(), Quaternion.identity);
        Destroy(effect, 5f);

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

        tower.UpgradeTower();

        GameObject effect = Instantiate(buildManager.buildEffect, GetBuildPosition(), Quaternion.identity);
        Destroy(effect, 5f);
        
        PlayerStats.UpdateMoney();
    }

    public void SellTurret()
    {
        // Add money at half the cost spent
        PlayerStats.Money += turretBlueprint.GetSellValue(tower.numberOfUpgrades);
        // Destroy turret and kill references
        Destroy(turret);
        turret = null;

        // Play effect
        GameObject effect = Instantiate(buildManager.sellEffect, GetBuildPosition(), Quaternion.identity);
        Destroy(effect, 5f);

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
        rend.material.color = startColor;
        if (buildManager.turretMenu.isActive == false)
        {
            range.Hide();
        } 
    }
}
