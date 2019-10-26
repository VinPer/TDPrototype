using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class SelectedTurrets : MonoBehaviour
{
    public static SelectedTurrets instance;
    // Keep the object to reference in the actual level so we can know which turrets are being used
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (instance != null)
        {
            instance.selectedTurrets = new List<TurretBlueprint>();
            Destroy(gameObject);
        }
        else
            instance = this;
    }
    private void Start()
    {
        selectedTurrets = new List<TurretBlueprint>();
        unlockedTurrets = new List<TurretBlueprint>();
        unlockedTurrets.Add(allTurrets.FirstOrDefault(x => Equals(x.name, "Basic")));
        unlockedTurrets.Add(allTurrets.FirstOrDefault(x => Equals(x.name, "Rocket")));
        unlockedTurrets.Add(allTurrets.FirstOrDefault(x => Equals(x.name, "Ice")));
        foreach (var item in UpgradeHandler.data.unlockedTowers)
        {
            if (item.Value)
                unlockedTurrets.Add(allTurrets.FirstOrDefault(x => Equals(x.name, item.Key)));
        }

        foreach(var dict in UpgradeHandler.data.shopUpgrades.Values)
        {
            foreach(var item in dict)
            {
                if (item.Value)
                    unlockedTurrets.Add(allTurrets.FirstOrDefault(x => Equals(x.name, item.Key)));
            }
        }

    }
    
    public List<TurretBlueprint> selectedTurrets;
    public List<TurretBlueprint> allTurrets;
    public List<TurretBlueprint> unlockedTurrets;
}
