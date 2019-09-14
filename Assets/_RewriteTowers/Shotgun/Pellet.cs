using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pellet : ProjectileBase
{
    public float rotationOffset;

    private void OnEnable()
    {
        StartCoroutine(Direction());
    }

    IEnumerator Direction()
    {
        yield return new WaitForEndOfFrame();
        direction = new Vector3(direction.x * Random.Range(0, rotationOffset), direction.y * Random.Range(0, rotationOffset), direction.z * Random.Range(0, rotationOffset));
    }
}
