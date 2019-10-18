using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UnlockTowerCanvas : MonoBehaviour
{
    [HideInInspector]
    public string turretUnlocked;

    public Image turretImage;
    public TextMeshProUGUI turretName;
    public GameObject turretAbout;
    public GameObject firstDialog;

    private enum TextMode { firstDialog, wait, turretInfo}
    private TextMode textMode = TextMode.firstDialog;

    private void Start()
    {
        firstDialog.SetActive(true);
        turretAbout.SetActive(false);
        SelectedTurrets.allTurrets.ForEach(item => {
            if (string.Equals(item.name, turretUnlocked))
            {
                turretImage.sprite = item.sprite;
                turretName.text = item.name;
                turretAbout.GetComponent<TextMeshProUGUI>().text = item.name;
            }
        });
    }

    private void Update()
    {
        if(Input.anyKey || Input.touchCount > 0)
        {
            if (textMode == TextMode.firstDialog)
            {
                firstDialog.SetActive(false);
                turretAbout.SetActive(true);
                textMode = TextMode.wait;
                return;
            }
            if (textMode == TextMode.turretInfo) gameObject.SetActive(false);
        }

        if (textMode == TextMode.wait && !Input.anyKey)
        {
            textMode = TextMode.turretInfo;
            return;
        }

    }
}
