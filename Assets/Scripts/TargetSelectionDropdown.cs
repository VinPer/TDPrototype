using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetSelectionDropdown : MonoBehaviour
{
    public Dropdown dropdown;
    private TowerProjectile towerProjectile;
    private TowerLaser towerLaser;

    public void UpdateDropdown()
    {
        dropdown.interactable = true;
        dropdown.ClearOptions();
        towerProjectile = GetComponent<TurretMenu>().turretSelected.GetComponent<TowerProjectile>();
        if (towerProjectile)
        {
            dropdown.AddOptions(towerProjectile.targetStyles);
            Debug.Log("projectlie");
            dropdown.value = towerProjectile.targetStyles.IndexOf(towerProjectile.targetSelected);
        }
        else
        {
            towerLaser = GetComponent<TurretMenu>().turretSelected.GetComponent<TowerLaser>();
            if (towerLaser)
            {
                dropdown.AddOptions(towerLaser.targetStyles);
                Debug.Log("lasers");
                dropdown.value = towerLaser.targetStyles.IndexOf(towerLaser.targetSelected);
            }
            else
            {
                dropdown.interactable = false;
            }
        }
        dropdown.RefreshShownValue();
    }

    public void Dropdown_IndexChanged(int index)
    {
        if (towerProjectile)
            towerProjectile.targetSelected = towerProjectile.targetStyles[index];
        else if (towerLaser)
            towerLaser.targetSelected = towerLaser.targetStyles[index];
    }
}
