using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerUpgrade : MonoBehaviour
{
    public Dictionary<string, int> towerBasic = new Dictionary<string, int> //1
    {
        { "range", 0},
        { "damage", 0},
        { "fireRate", 0},
        { "piercing", 0},
        {"penetration", 0 }
    };
    public Dictionary<string, int> towerRocket = new Dictionary<string, int> //5
    {
        { "range", 0},
        { "damage", 0},
        { "fireRate", 0},
        { "explosionRadius", 0}
    };
    public Dictionary<string, int> towerIce = new Dictionary<string, int> //8
    {
        { "range", 0},
        { "debuffIntensity", 0},
        { "debuffDuration", 0},
    };
    public Dictionary<string, int> towerAcid= new Dictionary<string, int> //8
    {
        { "range", 0},
        { "debuffIntensity", 0},
        { "debuffDuration", 0},
    };
    public Dictionary<string, int> towerRadar = new Dictionary<string, int> //8
    {
        { "range", 0},
    };
    public Dictionary<string, int> towerSniper = new Dictionary<string, int> //8
    {
        { "range", 0},
        { "damage", 0},
        { "fireRate", 0},
        { "penetration", 0},
    };
    public Dictionary<string, int> towerOverheat = new Dictionary<string, int> //4
    {
        { "range", 0},
        { "damage", 0},
        { "fireRate", 0},
        { "debuffIntensity", 0},
        { "debuffDuration", 0}
    };
    public Dictionary<string, int> towerLaser = new Dictionary<string, int> //4
    {
        { "range", 0},
        { "damage", 0},
        { "multiplierSpeed", 0}
    };
    public Dictionary<string, int> towerCharger = new Dictionary<string, int> //2
    {
        { "range", 0},
        { "damage", 0},
        { "chargeRate", 0},
        { "maxCharge", 0}
    };
    public Dictionary<string, int> towerTesla= new Dictionary<string, int> //2
    {
        { "range", 0},
        { "damage", 0},
        { "fireRate", 0},
        { "chainAmount", 0}
    };
    public Dictionary<string, int> flamethrower = new Dictionary<string, int> //2
    {
        { "damage", 0},
        { "debuffIntensity", 0},
        { "debuffDuration", 0}
    };
    public Dictionary<string, int> towerMortar = new Dictionary<string, int> //2
    {
        { "range", 0},
        { "damage", 0},
        { "fireRate", 0},
        { "explosionRadius", 0}
    };
    public Dictionary<string, int> towerBuffer = new Dictionary<string, int> //6
    {
        { "range", 0},
        { "buffRange", 0},
        { "buffDamage", 0},
        { "buffFireRate", 0}
    };
    public Dictionary<string, int> towerShotgun = new Dictionary<string, int> //6
    {
        { "range", 0},
        { "damage", 0},
        { "fireRate", 0},
        { "piercing", 0},
        { "projectileAmount", 0},
        { "spreadReduction", 0}
    };
    public Dictionary<string, int> towerGatling = new Dictionary<string, int> //3
    {
        { "range", 0},
        { "damage", 0},
        { "fireRate", 0},
        { "piercing", 0},
        { "penetration", 0}
    };
    public Dictionary<string, int> towerFreezer= new Dictionary<string, int> //3
    {
        { "damage", 0},
        { "chargeTime", 0}
    };
    public Dictionary<string, int> towerSpitter = new Dictionary<string, int> //8
    {
        { "range", 0},
        { "damage", 0},
        { "puddleDuration", 0},
        { "puddleSize", 0},
        { "debuffIntensity", 0},
        { "debuffDuration", 0}
    };

    public Dictionary<string, Dictionary<string, int>> towers;

    public static TowerUpgrade instance;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        towers = new Dictionary<string, Dictionary<string, int>>
            {
                { "Basic", towerBasic },
                { "Rocket", towerRocket },
                { "Laser", towerLaser },
                { "Megaman", towerCharger },
                { "Tesla", towerTesla },
                { "Flamethrower", flamethrower},
                { "Mortar", towerMortar },
                { "Radar", towerRadar },
                { "Buffer", towerBuffer },
                { "Shotgun", towerShotgun },
                { "Sniper", towerSniper },
                { "Acid", towerAcid },
                { "Ice", towerIce },
                { "Gatling", towerGatling },
                { "Freezer", towerFreezer },
                { "Spitter", towerSpitter },
                { "Overheat", towerOverheat }
            };
    }
}
