using UnityEngine;
using UnityEngine.EventSystems;

public class Node : MonoBehaviour
{
    public BuildManager buildManager;

    [HideInInspector]
    public GameObject turret;
    [HideInInspector]
    public TurretBlueprint turretBlueprint;
    [HideInInspector]
    public bool isUpgraded = false;

    public Vector3 positionOffset;

    private Renderer rend;
    private Color startColor;
    public Color hoverColor;
    public Color cannotAffordColor;
    
    public Range range;

    private void Start()
    {
        buildManager = BuildManager.instance;

        rend = GetComponent<Renderer>();
        startColor = rend.material.color;
        
        range = FindObjectOfType<Range>();
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
            return;
        }

        if (!buildManager.CanBuild) return;

        // buildManager.BuildTurretOn(this);
        BuildTurret(buildManager.GetTurretToBuild());
    }

    void BuildTurret (TurretBlueprint blueprint)
    {
        if (PlayerStats.Money < blueprint.cost)
        {
            Debug.Log("Not enough money to build that!");
            return;
        }

        PlayerStats.Money -= blueprint.cost;

        turret = Instantiate(blueprint.prefab, GetBuildPosition(), Quaternion.identity);
        turret.transform.SetParent(transform);

        turretBlueprint = blueprint;

        GameObject effect = Instantiate(buildManager.buildEffect, GetBuildPosition(), Quaternion.identity);
        Destroy(effect, 5f);

        Debug.Log("Turret built!");
        PlayerStats.UpdateMoney();
    }

    public void UpgradeTurret()
    {
        if (PlayerStats.Money < turretBlueprint.upgradeCost)
        {
            Debug.Log("Not enough money to upgrade that!");
            return;
        }

        PlayerStats.Money -= turretBlueprint.upgradeCost;

        // Get rid of old turret
        Destroy(turret);
        // Build new turret
        turret = Instantiate(turretBlueprint.upgradedPrefab, GetBuildPosition(), Quaternion.identity);
        turret.transform.SetParent(transform);

        GameObject effect = Instantiate(buildManager.buildEffect, GetBuildPosition(), Quaternion.identity);
        Destroy(effect, 5f);

        isUpgraded = true;
        Debug.Log("Turret upgraded!");
        PlayerStats.UpdateMoney();
    }

    public void SellTurret()
    {
        // Add money at half the cost spent
        PlayerStats.Money += turretBlueprint.GetSellValue(isUpgraded);
        // Destroy turret and kill references
        Destroy(turret);
        turret = null;
        isUpgraded = false;

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
        if (buildManager.nodeUI.target == null) range.Hide();
    }
}
