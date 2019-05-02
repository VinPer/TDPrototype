using UnityEngine;

public class Shop : MonoBehaviour
{
    BuildManager buildManager;

    public TurretBlueprint standardTurret;
    public TurretBlueprint missileLauncher;
    public TurretBlueprint bombSpawner;
    public TurretBlueprint flamethrower;
    public TurretBlueprint laserBeamer;

    private void Start()
    {
        buildManager = BuildManager.instance;
    }

    public void SelectStandardTurret()
    {
        Debug.Log("Standard Turret Selected");
        buildManager.SelectTurretToBuild(standardTurret);
    }

    public void SelectMissileLauncher()
    {
        Debug.Log("Missile Launcher Selected");
        buildManager.SelectTurretToBuild(missileLauncher);
    }

    public void SelectLaserBeamer()
    {
        Debug.Log("Laser Beamer Selected");
        buildManager.SelectTurretToBuild(laserBeamer);
    }

    public void SelectBombSpawner()
    {
        Debug.Log("Bomb Spawner Selected");
        buildManager.SelectTurretToBuild(bombSpawner);
    }

    public void SelectFlamethrower()
    {
        Debug.Log("Flamethrower Selected");
        buildManager.SelectTurretToBuild(flamethrower);
    }

    
}
