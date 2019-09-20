using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShop : MonoBehaviour
{
    private Vector3 rotationVector;
    public bool randomRotation = true;
    // Update is called once per frame

    private void Start()
    {
        rotationVector = new Vector3(Random.Range(-.2f, .2f), Random.Range(-.2f, .2f), Random.Range(-.2f, .2f));
        if(randomRotation == true)
            InvokeRepeating("RandomVector", 0, 3);
    }

    void Update()
    {
        transform.Rotate(rotationVector);
    }

    void RandomVector()
    {
        rotationVector = new Vector3(Random.Range(-.2f, .2f), Random.Range(-.2f, .2f), Random.Range(-.2f, .2f));
    }
}
