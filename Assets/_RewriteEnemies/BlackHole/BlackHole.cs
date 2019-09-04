using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHole : MonoBehaviour
{
    private ProjectileBase projectile;
    public GameObject blackHoleEnemy;
    void Start()
    {
        
    }
    void OnTriggerEnter(Collider other)
    {
        print("colisao!");
        projectile = other.transform.GetComponent<ProjectileBase>();

        if(projectile != null){
            print("eh um projetil!");
            projectile.transform.LookAt(blackHoleEnemy.transform);
            projectile.SetTarget(blackHoleEnemy.transform);
        }   
    }
}
