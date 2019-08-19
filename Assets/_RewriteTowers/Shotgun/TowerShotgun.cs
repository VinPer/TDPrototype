using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerShotgun : TowerProjectile
{
    //Numero de tiros menores para spawnar
    public int pelletCount = 5;
    //angulo máximo do "spread" do tiro
    public float rotationOffset = 10f;

    private List<GameObject> pellets;
    
    protected override void SpawnBulletPool()
    {
        System.Random rng = new System.Random();
        Quaternion rotation = Quaternion.Euler(new Vector3((float)rng.NextDouble() * rotationOffset, (float)rng.NextDouble() * rotationOffset, (float)rng.NextDouble() * rotationOffset));
        pellets = new List<GameObject>();
        
        for (int i = 0; i < pelletCount; i++)
        {
            GameObject newPellet = Instantiate(bulletPrefab, transform.position, rotation);
            newPellet.transform.SetParent(transform);
            newPellet.SetActive(false);
            pellets.Add(newPellet);
        }
    }

    protected override void Shoot()
    {
        for (int i = 0; i < pellets.Count; i++)
        {
            if (!pellets[i].activeInHierarchy)
            {
                pellets[i].transform.position = firePoint.position;
                pellets[i].transform.rotation = firePoint.rotation;
                pellets[i].SetActive(true);
                pellets[i].GetComponent<ProjectileBase>().damage *= damageBoost;
                pellets[i].GetComponent<ProjectileBase>().SetTarget(target);
            }
        }
    }

    protected override void UpgradeStatus()
    {
        throw new System.NotImplementedException();
    }
}
