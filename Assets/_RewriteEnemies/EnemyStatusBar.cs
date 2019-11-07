using UnityEngine;

public class EnemyStatusBar : MonoBehaviour
{
    public Camera mainCamera;

    private EnemyBase enemy;

    private bool invisible = false;

    private void Start()
    {
        mainCamera = FindObjectOfType<Camera>();
        enemy = GetComponentInParent<EnemyBase>();
    }

    void Update()
    {
        transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.forward, mainCamera.transform.rotation * Vector3.up);
        if (enemy == null) return;
        if (enemy.GetInvisibleState())
        {
            if (!invisible)
            {
                GetComponentInChildren<CanvasGroup>().alpha = .2f;
                invisible = true;
            }
        }
        else
        {
            if (invisible)
            {
                GetComponentInChildren<CanvasGroup>().alpha = 1f;
                invisible = false;
            }
        }
    }
}
