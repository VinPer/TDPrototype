using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ShopTurretItem : MonoBehaviour
{
    public Button button;
    public GameObject locker;
    public GameObject turretName;
    public GameObject turretPrice;

    // Update is called once per frame
    void Start()
    {
        StartCoroutine(IsLocked());
    }

    IEnumerator IsLocked()
    {
        yield return new WaitForEndOfFrame();
        if (!button.interactable)
        {
            locker.SetActive(true);
            turretName.SetActive(false);
            turretPrice.SetActive(false);
        }
        else
        {
            locker.SetActive(false);
            turretName.SetActive(true);
            turretPrice.SetActive(true);
        }
        if (Equals(SceneManager.GetActiveScene().name, "TowerUpgrades")) turretPrice.SetActive(false);
    }
}
