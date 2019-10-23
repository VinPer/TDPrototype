using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedLevel : MonoBehaviour
{
    public static SelectedLevel instance;
    // Keep the object to reference in the actual level so we can know which turrets are being used
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (instance != null)
        {
            //selectedTurrets = new List<TurretBlueprint>(instance.selectedTurrets);
            Destroy(gameObject);
        }
        else
            instance = this;
    }   

    private void Start()
    {
        selectedLevel = "";
    }
    public string selectedLevel { get; set; }
}
