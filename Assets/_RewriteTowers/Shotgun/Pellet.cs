﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pellet : ProjectileBase
{
    public float rotationOffset = 10f;

    private void OnEnable()
    {
        StartCoroutine(Direction());
    }

    IEnumerator Direction()
    {
        yield return new WaitForSeconds(.01f);
        direction = new Vector3(direction.x * Random.Range(0, rotationOffset), direction.y * Random.Range(0, rotationOffset), direction.z * Random.Range(0, rotationOffset));
    }
}
