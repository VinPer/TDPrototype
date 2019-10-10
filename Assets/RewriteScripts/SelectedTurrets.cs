using UnityEngine;
using System.Collections.Generic;

public class SelectedTurrets : MonoBehaviour
{
    public static SelectedTurrets instance;
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
        selectedTurrets = new List<TurretBlueprint>();
    }
    public List<TurretBlueprint> selectedTurrets;
    public static List<TurretBlueprint> allTurrets;
    public List<TurretBlueprint> unlockedTurrets;
}
