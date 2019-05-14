using System.Collections;
using UnityEngine;

public abstract class Skill : MonoBehaviour
{
    public float skillRadius = 5f;

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
        else Destroy(gameObject);
    }

    protected bool IsValidArea()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, skillRadius / 4);
        foreach (Collider collider in colliders)
        {
            if (collider.tag == "Ground")
            {
                return true;
            }
        }
        return false;
    }

    protected abstract void ActivateSkill();
}
