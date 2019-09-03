using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerMortar : TowerProjectile
{
    public Transform firingArea;
    public float explosionRadius = 5f;

    protected override void Start()
    {
        // initialize scale of firingArea in function of explosion radius to display the area it'll affect
        firingArea.localScale = new Vector3(explosionRadius * 4, 1f, explosionRadius * 4);
    }
    
    public void SetArea()
    {
        // allows player to select where tower will fire
        StartCoroutine(SetFiringArea());
    }

    private IEnumerator SetFiringArea()
    {
        target = null;
        // display the blast zone
        firingArea.gameObject.SetActive(true);

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

            // stop if the player clicks anywhere
            if (Input.GetMouseButtonDown(0)) moving = false;
        }

        // hide the blast zone
        firingArea.gameObject.SetActive(false);

        // check if the firing area is valid, if not retain the old firing area
        if (!IsValidArea()) firingArea.position = initialFiringAreaPosition;
        else
        {
            // assign target position to firing area
            target = firingArea;
            LockOnTarget();
            Debug.Log(target.name);
        }
    }

    // checks if there's collision with the ground within an appropriate range to allow for a valid firing area
    private bool IsValidArea()
    {
        //Collider[] colliders = Physics.OverlapSphere(firingArea.position, explosionRadius / 4);
        //foreach (Collider collider in colliders)
        //{
        //    if (collider.tag == "Ground")
        //    {
        //        return true;
        //    }
        //}
        //return false;
        return true;
    }
}
