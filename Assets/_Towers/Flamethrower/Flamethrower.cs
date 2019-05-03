using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flamethrower : MonoBehaviour
{
    public enum Direction { Clockwise = 1, Counter = -1};
    private Transform target;

    [Header("Attributes")]

    public float range = 15f;
    private float currentRotation = 0f;
    public Direction direction;

    [Header("Unity Setup Fields")]

    public string enemyTag = "Enemy";
    public Transform partToRotate;
    public float turnSpeed = 1f;

    public GameObject flamePrefab;
    public Transform firePoint;

    private GameObject flames;

    // Instantiates the flame item
    void Start()
    {
        Instantiate(flamePrefab, firePoint);
    }

    // Rotates the flamethrower at a constant rate dictated by the turnSpeed
    void Update()
    {
        RotateFlamethrower();
    }

    private void RotateFlamethrower()
    {
        partToRotate.rotation = Quaternion.Euler(0f, currentRotation * turnSpeed * 0.1f * (float) direction, 0f);
        currentRotation++;
        if (currentRotation * turnSpeed * 0.1f >= 360f) currentRotation = 0f;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
