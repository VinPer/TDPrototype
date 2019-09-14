using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerMortar : TowerProjectile
{
    public Transform firingArea;

    protected override void Start()
    {
        // initialize scale of firingArea in function of explosion radius to display the area it'll affect
        firingArea.localScale = new Vector3(explosionRadius * 4, 0.1f, explosionRadius * 4);
    }

    public void SetArea()
    {
        // allows player to select where tower will fire
        StartCoroutine(SetFiringArea());
    }

    private IEnumerator SetFiringArea()
    {
        target = null;
        // display the blast zone
        firingArea.gameObject.SetActive(true);

        float distanceToScreen;
        Vector3 posMove;
        bool moving = true;

        Vector3 initialFiringAreaPosition = firingArea.position;

        // move the firing area with the mouse
        while (moving)
        {
            distanceToScreen = Camera.main.WorldToScreenPoint(firingArea.position).z;
            posMove = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, distanceToScreen));
            firingArea.position = new Vector3(posMove.x, firingArea.position.y, posMove.z);

            yield return null;

            // stop if the player clicks anywhere
            if (Input.GetMouseButtonDown(0)) moving = false;
        }

        // hide the blast zone
        firingArea.gameObject.SetActive(false);

        // check if the firing area is valid, if not retain the old firing area
        if (!IsValidArea()) firingArea.position = initialFiringAreaPosition;
        else
        {
            // assign target position to firing area
            target = firingArea;
            LockOnTarget();
            Debug.Log(target.name);
        }
    }

    protected override void Shoot()
    {
        for (int i = 0; i < bullets.Count; i++)
        {
            if (!bullets[i].activeInHierarchy)
            {
                bullets[i].transform.position = firePoint.position;
                bullets[i].transform.rotation = firePoint.rotation;
                bullets[i].SetActive(true);
                UpdateBulletStatus(bullets[i].GetComponent<ProjectileBase>());
                bullets[i].GetComponent<ProjectileBase>().SetTarget(target);
                bullets[i].GetComponent<ProjectileMortar>().CalculateArc();
                if (GetComponentInParent<AudioSource>())
                    GetComponentInParent<AudioSource>().Play();
                break;
            }
        }
    }

    // checks if there's collision with the ground within an appropriate range to allow for a valid firing area
    protected bool IsValidArea()
    {
        //Collider[] colliders = Physics.OverlapCapsule(transform.position, transform.GetComponent<CapsuleCollider>().height);
        CapsuleCollider capsule = firingArea.GetComponent<CapsuleCollider>();
        Vector3 top = new Vector3(firingArea.position.x, firingArea.position.y + capsule.height / 2, firingArea.position.z);
        Vector3 bottom = new Vector3(firingArea.position.x, firingArea.position.y - capsule.height / 2, firingArea.position.z);
        Collider[] colliders = Physics.OverlapCapsule(top, bottom, capsule.radius);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Ground"))
            {
                return true;
            }
        }
        return false;
    }

    protected override void UpgradeStatus()
    {
        string _range = "range";
        if (upgrades[_range] < UpgradeHandler.data.towerUpgrades[transform.parent.name][_range])
        {
            range += rangeUpgrade;
            upgrades[_range]++;
            GetComponent<SphereCollider>().radius = range;
            print("range upgraded");
        }

        string _damage = "damage";
        if (upgrades[_damage] < UpgradeHandler.data.towerUpgrades[transform.parent.name][_damage])
        {
            damage += damageUpgrade;
            upgrades[_damage]++;
            print("damage upgraded");
        }
        string _radius = "explosionRadius";
        if (upgrades[_radius] < UpgradeHandler.data.towerUpgrades[transform.parent.name][_radius])
        {
            explosionRadius += explosionRadiusUpgrade;
            upgrades[_radius]++;
            print("explosionRadius upgraded");
        }

        //FAZENDO UPGRADE DA QUANTIDADE DE TIROS QUE ELE DA
        string _fireRate = "fireRate";
        if (upgrades[_fireRate] < UpgradeHandler.data.towerUpgrades[transform.parent.name][_fireRate])
        {
            //fireRate += fireRateUpgrade;
            poolAmount++;
            upgrades[_fireRate]++;
            //print("fireRate upgraded");
        }
    }
}
