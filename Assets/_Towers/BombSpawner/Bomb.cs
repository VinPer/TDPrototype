using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public float timeToDetonate = 2f;
    public float explosionRadius = 10f;
    public float damage = 50f;
    public float piercingValue = 100f;
    public GameObject explosionEffect;

    private bool bombPrimed = false;

    // If the bomb is primed after clicking and dragging, it detonates after timeToDetonate seconds
    void Update()
    {
        if(bombPrimed)
        {
            timeToDetonate -= Time.deltaTime;
        }

        if(timeToDetonate <= 0f)
        {
            Explode();
        }
    }

    // Allows the player to click and drag the bomb throughout the map to select where to drop it
    private void OnMouseDrag()
    {
        if (bombPrimed) return;

        float distance_to_screen = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
        Vector3 pos_move = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance_to_screen));
        transform.position = new Vector3(pos_move.x, transform.position.y, pos_move.z);
    }
    
    // Primes the bomb for explosion after the player lets go of the mouse click
    private void OnMouseUp()
    {
        Debug.Log("Bomb primed!");
        bombPrimed = true;
    }
    
    // Spawns explosion particle effects and damages all enemies within range before destroying the bomb
    private void Explode()
    {
        GameObject effectIns = (GameObject)Instantiate(explosionEffect, transform.position, transform.rotation);
        Destroy(effectIns, 5f);

        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider collider in colliders)
        {
            if (collider.tag == "Enemy")
            {
                Damage(collider.transform);
            }
        }

        Destroy(gameObject);
    }
    
    private void Damage(Transform enemy)
    {
        Enemy e = enemy.GetComponent<Enemy>();
        e.TakeDamage(damage, piercingValue);
    }
}
