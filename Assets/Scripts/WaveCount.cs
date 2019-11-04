using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WaveCount : MonoBehaviour
{
    int nWaves;

    public static WaveCount instance;
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one WaveCount in scene!");
            return;
        }

        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        nWaves = FindObjectOfType<WaveSpawner>().waves.Length;
        UpdateWaveCount();
    }

    // Update is called once per frame
    public void UpdateWaveCount()
    {
        GetComponent<TextMeshProUGUI>().text = (PlayerStats.WavesSurvived + 1) + "/" + nWaves; 
    }
}
