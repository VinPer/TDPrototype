using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class PlayerData
{
    public Dictionary<string, int> playerStats;
    public Dictionary<string, int> levelsClear;
    public Dictionary<string, Dictionary<string, bool>> shopUpgrades;
    public Dictionary<string, Dictionary<string, int>> towerUpgrades;

    public void Reset()
    {
        playerStats = new Dictionary<string, int>
        {
            { "Coins", 0 },
            { "TotalStars", 0 },
            { "UnspentStars", 0 }
        };
        levelsClear = new Dictionary<string, int>
        {
            { "1", 0 },
            { "2", 0 },
            { "3", 0 },
            { "4", 0 },
            { "5", 0 },
            { "6", 0 },
            { "7", 0 },
        };
        shopUpgrades = new Dictionary<string, Dictionary<string, bool>>
        {
            { "Block1", new Dictionary<string, bool>
            {
                { "MoreTowers", false },
                { "MoreSkills", false },
                { "ElementalBlast", false },
                { "Mortar", false },
                { "Shotgun", false },
                { "Gatling", false }
            } },
            { "Block2", new Dictionary<string, bool>
            {
                { "MoreTowersPlus", false },
                { "MoreSkillsPlus", false },
                { "BuffAllTowers", false },
                { "Buffer", false },
                { "Tesla", false }
            } },
            { "Block3", new Dictionary<string, bool>
            {
                { "Flamethrower", false },
                { "Freezer", false },
                { "Spitter", false },
                { "UltimateTower", false },
                { "Nuke", false }
            } },
        };
        towerUpgrades = new Dictionary<string, Dictionary<string, int>>
        {
            { "Basic", new Dictionary<string, int> //1
            {
                { "range", 0 },
                { "damage", 0 },
                { "fireRate", 0 },
                { "piercing", 0 },
                { "penetration", 0 }
            } },
            { "Rocket", new Dictionary<string, int> //1
            {
                { "range", 0 },
                { "damage", 0 },
                { "fireRate", 0 },
                { "explosionRadius", 0 }
            } },
            { "Ice", new Dictionary<string, int> //1
            {
                { "range", 0 },
                { "debuffIntensity", 0 },
                { "debuffDuration", 0 }
            } },
            { "Acid", new Dictionary<string, int> //1
            {
                { "range", 0 },
                { "debuffIntensity", 0 },
                { "debuffDuration", 0 }
            } },
            { "Radar", new Dictionary<string, int> //1
            {
                { "range", 0 }
            } },
            { "Sniper", new Dictionary<string, int> //1
            {
                { "range", 0 },
                { "damage", 0 },
                { "fireRate", 0 },
                { "penetration", 0 }
            } },
            { "Overheat", new Dictionary<string, int> //1
            {
                { "range", 0 },
                { "damage", 0 },
                { "fireRate", 0 },
                { "debuffIntensity", 0 },
                { "debuffDuration", 0 }
            } },
            { "Laser", new Dictionary<string, int> //1
            {
                { "range", 0 },
                { "damage", 0 },
                { "multiplierSpeed", 0 }
            } },
            { "Charger", new Dictionary<string, int> //1
            {
                { "range", 0 },
                { "damage", 0 },
                { "chargeRate", 0 },
                { "maxCharge", 0 }
            } },
            { "Tesla", new Dictionary<string, int> //1
            {
                { "range", 0 },
                { "damage", 0 },
                { "fireRate", 0 },
                { "chainAmount", 0 }
            } },
            { "Flamethrower", new Dictionary<string, int> //1
            {
                { "damage", 0 },
                { "debuffIntensity", 0 },
                { "debuffDuration", 0 }
            } },
            { "Mortar", new Dictionary<string, int> //1
            {
                { "range", 0 },
                { "damage", 0 },
                { "fireRate", 0 },
                { "explosionRadius", 0 }
            } },
            { "Buffer", new Dictionary<string, int> //1
            {
                { "range", 0 },
                { "buffRange", 0 },
                { "buffDamage", 0 },
                { "buffFireRate", 0 }
            } },
            { "Shotgun", new Dictionary<string, int> //1
            {
                { "range", 0 },
                { "damage", 0 },
                { "fireRate", 0 },
                { "piercing", 0 },
                { "projectileAmount", 0 },
                { "spreadReduction", 0 }
            } },
            { "Gatling", new Dictionary<string, int> //1
            {
                { "range", 0 },
                { "damage", 0 },
                { "fireRate", 0 },
                { "piercing", 0 },
                { "penetration", 0 }
            } },
            { "Freezer", new Dictionary<string, int> //1
            {
                { "damage", 0 },
                { "chargeTime", 0 }
            } },
            { "Spitter", new Dictionary<string, int> //1
            {
                { "range", 0 },
                { "damage", 0 },
                { "puddleDuration", 0 },
                { "puddleSize", 0 },
                { "debuffIntensity", 0 },
                { "debuffDuration", 0 }
            } }
        };
    }
}
