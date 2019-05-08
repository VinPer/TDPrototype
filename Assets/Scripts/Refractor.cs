using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Refractor : MonoBehaviour
{
    public Transform parentBox;
    public Transform spawnPoint;

    private void OnTriggerEnter(Collider col)
    {
        Transform bullet = col.transform;
        if (bullet.tag != "Bullet") return;

        Refractor currentChild;
        for(int i = 0; i < parentBox.childCount; i++)
        {
            if (transform.GetSiblingIndex() != i)
            {
                currentChild = parentBox.GetChild(i).GetComponent<Refractor>();
                SpawnBullet(bullet, currentChild);
            }
        }

        Destroy(bullet.gameObject);
    }

    private void SpawnBullet(Transform bullet, Refractor side)
    {
        Transform b = Instantiate(bullet, side.spawnPoint.position, side.spawnPoint.rotation);
        LinearBullet lb = b.GetComponent<LinearBullet>();
        lb.Seek(side.GetDirection());
        lb.initialDecayTimer *= 0.75f;
        lb.decayTimer = lb.initialDecayTimer;
    }

    public Vector3 GetDirection()
    {
        return spawnPoint.position - transform.position;
    }
}
