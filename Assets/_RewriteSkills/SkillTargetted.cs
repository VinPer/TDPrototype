using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkillTargetted : SkillBase
{
    public float radius;
    public float yToSpawn;
    protected bool placed;

    protected void Start()
    {
        StartCoroutine(MoveSkill());
    }

    protected IEnumerator MoveSkill()
    {
        float distanceToScreen;
        Vector3 posMove;
        bool moving = true;

        Vector3 initialPosition = base.transform.position;
        // move the firing area with the mouse
        while (moving)
        {
            distanceToScreen = Camera.main.WorldToScreenPoint(transform.position).z;
            posMove = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, distanceToScreen));
            transform.position = new Vector3(posMove.x, transform.position.y, posMove.z);

            yield return null;

            // stop if the player clicks anywhere
            if (Input.GetMouseButtonDown(0)) moving = false;
        }

        if (IsValidArea()) ActivateSkill();
        else
        {
            Destroy(gameObject);
            // manage cooldown
        }
    }

    protected bool IsValidArea()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius / 4);
        foreach (Collider collider in colliders)
        {
            if (collider.tag == "Ground")
            {
                return true;
            }
        }
        return false;
    }

    protected void ActivateSkill()
    {
        placed = true;
    }
}
