using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinearTurret : Turret
{
    protected bool selectingRotation = false;
    public Transform firingArea;

    protected override void Start()
    {
        target = firingArea;
    }

    protected override void Update()
    {
        if (selectingRotation) return;

        if (fireCountdown <= 0f)
        {
            Shoot();
            fireCountdown = 1f / fireRate;
        }

        fireCountdown -= Time.deltaTime;
    }

    protected void OnMouseDown()
    {
        // allows player to select where tower will fire
        if (!selectingRotation)
        {
            selectingRotation = true;
            StartCoroutine(SetFiringArea());
        }
    }

    protected IEnumerator SetFiringArea()
    {
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
        
        selectingRotation = false;
    }
}
