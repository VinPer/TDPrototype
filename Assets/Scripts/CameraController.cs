﻿using UnityEngine;

public class CameraController : MonoBehaviour
{
    private bool enableMovement = false;

    public float panSpeed = 30f;
    public float panBorderThickness = 10f;

    public float rotationSpeed = 100f;

    public float scrollSpeed = 5f;
    public float minY = 10f;
    public float maxY = 130f;
    public float minX = 5f;
    public float maxX = 70f;
    public float minZ = 0f;
    public float maxZ = 70;

    Rect screenRect = new Rect(0, 0, Screen.width, Screen.height);

    // Update is called once per frame
    void Update()
    {
        if (GameManager.GameIsOver)
        {
            enableMovement = false;
            return;
        }

        //mouse out of the screen
        if (!screenRect.Contains(Input.mousePosition)) return;

        if (Input.GetKeyDown(KeyCode.X)) enableMovement = !enableMovement;
        if (!enableMovement) return;

        //Rotation
        if (Input.GetKey("q"))
        {
            transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime, Space.Self);
        }
        if (Input.GetKey("e"))
        {
            transform.Rotate(Vector3.down * rotationSpeed * Time.deltaTime, Space.Self);
        }

        //Translation
        if (Input.GetKey("w") || Input.mousePosition.y >= Screen.height - panBorderThickness)
        {
            transform.Translate(Vector3.forward * panSpeed * Time.deltaTime, Space.World);
        }
        if (Input.GetKey("s") || Input.mousePosition.y <= panBorderThickness)
        {
            transform.Translate(Vector3.back * panSpeed * Time.deltaTime, Space.World);
        }
        if (Input.GetKey("d") || Input.mousePosition.x >= Screen.width - panBorderThickness)
        {
            transform.Translate(Vector3.right * panSpeed * Time.deltaTime, Space.World);
        }
        if (Input.GetKey("a") || Input.mousePosition.x <= panBorderThickness)
        {
            transform.Translate(Vector3.left * panSpeed * Time.deltaTime, Space.World);
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");

        Vector3 pos = transform.position;
        pos.y -= scroll * 1000 * scrollSpeed * Time.deltaTime;
        pos.y = Mathf.Clamp(pos.y, minY, maxY);
        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.z = Mathf.Clamp(pos.z, minZ, maxZ);
        transform.position = pos;
    }
}
