using UnityEngine;
using UnityEngine.UI;

public class SpeedButton : MonoBehaviour
{
    [HideInInspector]
    public bool speedUp;
    public Sprite normalSpeed;
    public Sprite fastSpeed;
    public int speedUpMultiplier = 2;
    private Image image;

    public static SpeedButton instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one SpeedButton in scene!");
            return;
        }

        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        NormalSpeed();
        image = GetComponent<Button>().image;
    }

    public void ToggleSpeed()
    {
        speedUp = (!speedUp);

        if (speedUp)
        {
            Time.timeScale = speedUpMultiplier;
            image.sprite = fastSpeed;
        }
        else
        {
            Time.timeScale = 1f;
            image.sprite = normalSpeed;
        }
    }
    public void NormalSpeed()
    {
        speedUp = false;
        Time.timeScale = 1f;
    }
}
