using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charger : MonoBehaviour
{
    public float chargeRate = 5f;
    public int maxOrbAmount = 3;
    public Vector3 offset;

    private float turnSpeed;
    private float cooldown = 1f;
    [HideInInspector]
    public float currentOrbAmount = 0;
    private float currentRotation = 0f;

    public Transform firePoint;
    public Transform orbPrefab;
    public GameObject chargeFX;

    private void Start()
    {
        turnSpeed = 360f / (chargeRate * maxOrbAmount);
    }

    private void Update()
    {
        if (cooldown <= 0f && currentOrbAmount < maxOrbAmount)
        {
            SpawnOrb();
            cooldown = chargeRate;
        }

        cooldown -= Time.deltaTime;
        //RotateOrbs();
    }

    private void SpawnOrb()
    {
        currentOrbAmount++;
        Transform orb = Instantiate(orbPrefab, firePoint.position + offset, Quaternion.identity);
        orb.SetParent(firePoint);
    }


    private void RotateOrbs()
    {
        firePoint.rotation = Quaternion.Euler(0f, currentRotation * turnSpeed, 0f);
        currentRotation += Time.deltaTime;
        if (currentRotation * turnSpeed * 0.1f >= 360f) currentRotation = 0f;
    }

}
