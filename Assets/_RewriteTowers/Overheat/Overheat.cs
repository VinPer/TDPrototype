using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Overheat : TowerProjectile
{
    private Transform firePoints;
    public Transform rotationPart;
    private Transform[] fps = new Transform[6];

    protected override void Start()
    {
        targetStyles = null;
        firePoints = rotationPart.GetChild(0);
        for (int i = 0; i < fps.Length; i++)
        {
            fps[i] = firePoints.GetChild(i).GetComponent<Transform>();
        }
        GetComponent<SphereCollider>().radius = range;
        possibleTargets = new List<GameObject>();
    }
    protected override void Update()
    {
        if (possibleTargets.Count == 0) return;
        if (fireCountdown <= 0f)
        {
            Shoot();
            fireCountdown = 1f / fireRate;
        }

        fireCountdown -= Time.deltaTime;
    }
    

    protected override void Shoot()
    {
        foreach(Transform fp in fps)
        {
            for (int i = 0; i < bullets.Count; i++)
            {
                if (!bullets[i].activeInHierarchy)
                {
                    Transform t = bullets[i].transform;
                    ProjectileBase p = bullets[i].GetComponent<ProjectileBase>();
                    t.position = new Vector3(fp.position.x,possibleTargets[0].transform.position.y,fp.position.z);
                    t.rotation = fp.rotation;
                    bullets[i].SetActive(true);
                    p.damage *= damageBoost;
                    p.SetTarget(possibleTargets[0].transform);
                    Vector3 dir = fp.GetChild(0).transform.position - t.position;
                    dir.y = 0;
                    p.SetDirection(dir);
                    //Debug.Log(p.GetDirection());
                    break;
                }
            }
        }
        rotationPart.Rotate(new Vector3(0, 30, 0));
        if (GetComponent<AudioSource>())
            GetComponent<AudioSource>().Play();
        List<GameObject> backup = new List<GameObject>(possibleTargets);
        foreach (GameObject item in backup)
        {
            if (!item.gameObject.activeSelf)
                possibleTargets.Remove(item);
        }
    }
}
