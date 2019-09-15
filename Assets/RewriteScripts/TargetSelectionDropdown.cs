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
        dropdown.gameObject.SetActive(true);
        dropdown.ClearOptions();
        towerProjectile = GetComponent<TurretMenu>().turretSelected.GetComponent<TowerProjectile>();
        if (towerProjectile && towerProjectile.targetStyles != null)
        {
            dropdown.AddOptions(towerProjectile.targetStyles);
            dropdown.value = towerProjectile.targetStyles.IndexOf(towerProjectile.targetSelected);
        }
        else
        {
            towerLaser = GetComponent<TurretMenu>().turretSelected.GetComponent<TowerLaser>();
            if (towerLaser && towerLaser.targetStyles != null)
            {
                dropdown.AddOptions(towerLaser.targetStyles);
                dropdown.value = towerLaser.targetStyles.IndexOf(towerLaser.targetSelected);
            }
            else
            {
                dropdown.interactable = false;
                dropdown.gameObject.SetActive(false);
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
