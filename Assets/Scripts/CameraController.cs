using UnityEngine;

public class CameraController : MonoBehaviour
{
    private bool enableMovement = true;

    Rect screenRect = new Rect(0, 0, Screen.width, Screen.height);

    [Header("Movement")]
    public float panSpeed = 30f;
    public float panBorderThickness = 10f;
    //public float rotationSpeed = 100f;
    public float scrollSpeed = 5f;

    [Header("Limits")]
    public float minY = 10f;
    public float maxY = 55f;
    public float minX = 5f;
    public float maxX = 70f;
    public float minZ = 0f;
    public float maxZ = 70;

    private float scroll;
    private Vector3 pos;

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

        //if (Input.GetKeyDown(KeyCode.X)) enableMovement = !enableMovement;
        if (!enableMovement) return;

        ////Rotation
        //if (Input.GetKey("q"))
        //{
        //    transform.Rotate(Vector3.down * rotationSpeed * Time.deltaTime, Space.Self);
        //}
        //if (Input.GetKey("e"))
        //{
        //    transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime, Space.Self);
        //}

        //Translation
        if (Input.GetKey("w") || Input.mousePosition.y >= Screen.height - panBorderThickness)
        {
            //vector3.foward = new Vector3(0f,0f,1f)
            transform.Translate(Vector3.forward * panSpeed * Time.deltaTime, Space.Self);
        }
        if (Input.GetKey("s") || Input.mousePosition.y <= panBorderThickness)
        {
            transform.Translate(Vector3.back * panSpeed * Time.deltaTime, Space.Self);
        }
        if (Input.GetKey("d") || Input.mousePosition.x >= Screen.width - panBorderThickness)
        {
            transform.Translate(Vector3.right * panSpeed * Time.deltaTime, Space.Self);
        }
        if (Input.GetKey("a") || Input.mousePosition.x <= panBorderThickness)
        {
            transform.Translate(Vector3.left * panSpeed * Time.deltaTime, Space.Self);
        }
        //Zoom
        scroll = Input.GetAxis("Mouse ScrollWheel");
        pos = transform.position;
        pos.y -= scroll * 1000 * scrollSpeed * Time.deltaTime;
        pos.y = Mathf.Clamp(pos.y, minY, maxY);
        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.z = Mathf.Clamp(pos.z, minZ, maxZ);
        transform.position = pos;

        //Zoom 2
        if (Input.touchCount == 2)
        {
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

            pos = transform.position;
            pos.y -= deltaMagnitudeDiff * scrollSpeed * 0.3f * Time.deltaTime;
            pos.y = Mathf.Clamp(pos.y, minY, maxY);
            pos.x = Mathf.Clamp(pos.x, minX, maxX);
            pos.z = Mathf.Clamp(pos.z, minZ, maxZ);
            transform.position = pos;
        }

        if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;
            transform.Translate(-touchDeltaPosition.x * panSpeed * 0.01f * Time.deltaTime, 0, -touchDeltaPosition.y * panSpeed * 0.01f * Time.deltaTime);

            pos = transform.position;
            pos.y = Mathf.Clamp(pos.y, minY, maxY);
            pos.x = Mathf.Clamp(pos.x, minX, maxX);
            pos.z = Mathf.Clamp(pos.z, minZ, maxZ);
        }
    }
}
