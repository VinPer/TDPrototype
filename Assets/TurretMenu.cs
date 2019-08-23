using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TurretMenu : MonoBehaviour
{
    private Node target;
    public TowerBase turretSelected;
    public TowerProjectile towerProjectile;
    public TowerNonProjectile towerNonProjectile;
    public TowerRadar towerRadar;
    public Image turretImage;
    public Text dmgText;
    public Text rangeText;
    public Image elementImage;
    public Animator anim;
    public Text btnUpgradeText;
    public Text btnSellText;

    public List<TurretBlueprint> turretBlueprints;
    public TurretBlueprint standardTurret;
    public TurretBlueprint missileLauncher;
    public TurretBlueprint bombSpawner;
    public TurretBlueprint flamethrower;
    public TurretBlueprint laserBeamer;

    public bool isActive = false;


    private void Start()
    {
        if (TurretHandler.active)
            turretBlueprints = TurretHandler.selectedTurrets;
        else
            LoadDefault();
        anim = gameObject.GetComponent<Animator>();
    }

    public void SetTarget(Node _target)
    {
        this.target = _target;
        turretImage.sprite = target.turretBlueprint.sprite;
        //Debug.Log(target.GetComponentInChildren<TowerBase>().GetComponent<TowerProjectile>().bulletPrefab.GetComponent<ProjectileBase>().damage);
        turretSelected = target.GetComponentInChildren<TowerBase>();
        rangeText.text = turretSelected.range.ToString();

        towerProjectile = turretSelected.GetComponent<TowerProjectile>();
        if (towerProjectile != null)
        {
            dmgText.text = towerProjectile.bulletPrefab.GetComponent<ProjectileBase>().damage.ToString();
            //elementImage.sprite = target.turretBlueprint.element; << 
        }
        else if(turretSelected.GetComponent<TowerNonProjectile>())
        {
            towerNonProjectile = turretSelected.GetComponent<TowerNonProjectile>();
            dmgText.text = towerNonProjectile.damage.ToString();
            //elementImage.sprite = target.turretBlueprint.element; << 
        }
        else
        {
            towerRadar = turretSelected.GetComponent<TowerRadar>();
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
        Debug.Log(anim);
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
