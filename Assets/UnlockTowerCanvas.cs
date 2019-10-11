using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnlockTowerCanvas : MonoBehaviour
{
    public Image turretImage;
    public Text turretName;
    public Text turretAbout;

    private void Update()
    {
        if(Input.anyKey || Input.touchCount > 0)
            gameObject.SetActive(false);
    }
}
