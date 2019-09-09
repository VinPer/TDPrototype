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
        projectile = other.transform.GetComponent<ProjectileBase>();

        if(projectile != null){
            projectile.transform.LookAt(blackHoleEnemy.transform);
            projectile.SetTarget(blackHoleEnemy.transform);
            projectile.SetSpeed(projectile.speed * .75f);
        }   
    }
}
