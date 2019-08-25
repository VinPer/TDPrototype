using UnityEngine;
using UnityEngine.UI;

public class NodeUI : MonoBehaviour
{
    public Node target;
    public GameObject ui;
    public Text upgradeCost;
    public Button upgradeButton;
    public Text sellValue;

    public void SetTarget(Node target)
    {
        this.target = target;

        //transform.position = this.target.GetBuildPosition();
        if (!target.GetComponentInChildren<TowerBase>().turretMaximized)
        {
            upgradeCost.text = "$" + target.turretBlueprint.upgradeCost;
            //sellValue.text = "$" + target.turretBlueprint.GetSellValue(false);
            upgradeButton.interactable = true;
        }
        else
        {
            upgradeCost.text = "MAXIMIZED";
            //sellValue.text = "$" + target.turretBlueprint.GetSellValue(true);
            upgradeButton.interactable = false;
        }

        ui.SetActive(true);
        target.range.SetTarget(target);
    }

    public void Upgrade()
    {
        target.UpgradeTurret();
        BuildManager.instance.DeselectNode();
    }

    public void Sell()
    {
        target.SellTurret();
        BuildManager.instance.DeselectNode();
    }

    public void Hide()
    {
        if(target != null)
        {
            target.range.Hide();
            target = null;
        }
        ui.SetActive(false);
    }
}
