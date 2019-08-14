using System.Collections;
using UnityEngine;

public class TowerFlamethrower : TowerNonProjectile
{
    public enum Direction { Clockwise = 1, Counter = -1, Motionless = 0 };
    public Transform firingArea;

    [Header("Attributes")]
    
    private float currentRotation = 0f;
    public Direction direction;

    [Header("Unity Setup Fields")]

    public string enemyTag = "Enemy";
    public Transform partToRotate;
    public float turnSpeed = 1f;
    
    public Transform firePoint;

    public GameObject flames;

    private bool selectingRotation = false;

    // Rotates the flamethrower at a constant rate dictated by the turnSpeed
    void Update()
    {
        if (direction != 0) RotateFlamethrower();
    }

    private void RotateFlamethrower()
    {
        partToRotate.rotation = Quaternion.Euler(0f, currentRotation * turnSpeed * 0.1f * (float)direction, 0f);
        currentRotation++;
        if (currentRotation * turnSpeed * 0.1f >= 360f) currentRotation = 0f;
    }

    private void OnMouseDown()
    {
        // allows player to select where tower will fire
        if (!selectingRotation && direction == 0)
        {
            selectingRotation = true;
            StartCoroutine(SetFiringArea());
        }
    }

    private IEnumerator SetFiringArea()
    {
        // stop flames to prevent abuse
        flames.SetActive(false);

        float distanceToScreen;
        Vector3 posMove;
        bool moving = true;

        Vector3 initialFiringAreaPosition = firingArea.position;

        // move the firing area with the mouse
        while (moving)
        {
            distanceToScreen = Camera.main.WorldToScreenPoint(firingArea.position).z;
            posMove = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, distanceToScreen));
            firingArea.position = new Vector3(posMove.x, firingArea.position.y, posMove.z);

            yield return null;

            partToRotate.LookAt(firingArea);

            // stop if the player clicks anywhere
            if (Input.GetMouseButtonDown(0)) moving = false;
        }

        // activate flames again
        flames.SetActive(true);
        selectingRotation = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }

    protected override void UpgradeStatus()
    {
        range += rangeUpgrade;
    }

}
