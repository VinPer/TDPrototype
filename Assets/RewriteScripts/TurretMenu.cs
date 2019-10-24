using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TurretMenu : MonoBehaviour
{
    private Node target;
    [HideInInspector]
    public TowerBase turretSelected;
    [HideInInspector]
    public TowerProjectile towerProjectile;
    [HideInInspector]
    public TowerNonProjectile towerNonProjectile;
    public Image turretImage;
    public Text dmgText;
    public Text rangeText;
    public Image elementImage;
    [HideInInspector]
    public Animator anim;
    public Button btnUpgrade;
    public Text btnUpgradeText;
    public Text btnSellText;
    public Text turretName;

    public List<TurretBlueprint> turretBlueprints;
    public TurretBlueprint standardTurret;
    public TurretBlueprint missileLauncher;
    public TurretBlueprint bombSpawner;
    public TurretBlueprint flamethrower;
    public TurretBlueprint laserBeamer;

    public Sprite fire;
    public Sprite acid;
    public Sprite ice;
    public Sprite none;
    [HideInInspector]
    public bool isActive = false;

    public static TurretMenu instance;
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one turret menu in scene!");
            return;
        }

        instance = this;
        gameObject.SetActive(false);
    }

    private void Start()
    {
        if (TurretHandler.active)
            turretBlueprints = SelectedTurrets.instance.selectedTurrets;
        else
            LoadDefault();
        anim = gameObject.GetComponent<Animator>();
    }

    public void SetTarget(Node _target)
    {
        this.target = _target;
        turretImage.sprite = target.turretBlueprint.sprite;
        turretName.text = target.turretBlueprint.name;
        //Debug.Log(target.GetComponentInChildren<TowerBase>().GetComponent<TowerProjectile>().bulletPrefab.GetComponent<ProjectileBase>().damage);
        turretSelected = target.GetComponentInChildren<TowerBase>();
        if (turretSelected == null) turretSelected = target.transform.Find("Range").GetComponentInChildren<TowerBase>();
        GetComponent<TargetSelectionDropdown>().UpdateDropdown();
        rangeText.text = turretSelected.range.ToString();
        print(turretSelected.turretMaximized);
        if (turretSelected.turretMaximized)
        {
            btnUpgrade.interactable = false;
            btnUpgradeText.text = "MAXIMIZED";
        }
        else
        {
            btnUpgrade.interactable = true;
            btnSellText.text = "$" + target.turretBlueprint.GetSellValue(turretSelected.numberOfUpgrades);
        }
        btnUpgradeText.text ="$" + target.turretBlueprint.GetUpgradeCost();

        //Element Sprite
        TowerBase tb = target.turretBlueprint.prefab.GetComponent<TowerBase>();
        if (tb == null) tb = target.turretBlueprint.prefab.GetComponentInChildren<TowerBase>();
        switch (tb.element)
        {
            case Enums.Element.fire:
                elementImage.sprite = fire;
                elementImage.color = Color.red;
                break;
            case Enums.Element.acid:
                elementImage.sprite = acid;
                elementImage.color = Color.green;
                break;
            case Enums.Element.ice:
                elementImage.sprite = ice;
                elementImage.color = Color.cyan;
                break;
            case Enums.Element.none:
                elementImage.sprite = none;
                break;
        }
        towerProjectile = turretSelected.GetComponent<TowerProjectile>();
        if (towerProjectile != null)
        {
            dmgText.text = towerProjectile.bulletPrefab.GetComponent<ProjectileBase>().damage.ToString();
            dmgText.text = towerProjectile.damage.ToString();
        }
        else if(turretSelected.GetComponent<TowerNonProjectile>())
        {
            towerNonProjectile = turretSelected.GetComponent<TowerNonProjectile>();
            dmgText.text = towerNonProjectile.damage.ToString();
        }
        else
        {
            dmgText.text = "-";
        }

        //btnSellText.text ="<b>UPGRADE</b>\n" + target.turretBlueprint.GetSellValue();
        //btnUpgradeText.text ="<b>SELL</b>\n" + target.turretBlueprint.GetUpgradePrice();

        this.gameObject.SetActive(true);
        isActive = true;
        if(anim!=null)
            anim.Play("TurretMenuSlideIn");
    }
    public void CloseMenu()
    {
        Range.instance.Hide();
        isActive = false;
        if(anim!=null)
            anim.Play("TurretMenuSlideOut");
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

    // draw the turrets that are up for sale
    private void DrawTurrets()
    {
        Transform currentChild;

        // draw all from their data contained in TurretBlueprint
        for (int i = 0; i < turretBlueprints.Count; i++)
        {
            currentChild = transform.GetChild(i);
            currentChild.GetComponent<Image>().sprite = turretBlueprints[i].sprite;
            currentChild.GetComponent<Image>().color = turretBlueprints[i].color;
            currentChild.GetComponentInChildren<Text>().text = "$" + turretBlueprints[i].cost;
        }

        // draw the empty spots
        for (int i = turretBlueprints.Count; i < transform.childCount; i++)
        {
            currentChild = transform.GetChild(i);
            currentChild.GetComponent<Image>().color = new Color(0, 0, 0, 0);
            currentChild.GetComponentInChildren<Text>().text = "-";
        }
    }


}
