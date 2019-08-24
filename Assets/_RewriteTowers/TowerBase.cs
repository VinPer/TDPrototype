using UnityEngine;

public abstract class TowerBase : MonoBehaviour
{
    //[HideInInspector]
    //public BuildManager buildManager;
    
    protected Enums.Status currentState = Enums.Status.enable;
    public float range = 15f;
    protected float initialRange;
    protected float cost = 100f;
    protected int level = 1;
    public bool seesInvisible = false;

    public float rangeUpgrade = 2f;
    [HideInInspector]
    public int numberOfUpgrades = 0;
    public int maxUpgrade = 3;
    [HideInInspector]
    public bool turretMaximized = false;

    public Enums.Element element;

    protected float rangeBoost = 1f;
    protected float rateBoost = 1f;
    protected float damageBoost = 1f;

    //public TowerModel model;s

    protected virtual void Awake()
    {
        initialRange = range;    
    }
    
    protected virtual void OnEnable()
    {   
        rangeBoost = 1f;
        rateBoost = 1f;
        damageBoost = 1f;
        range = initialRange;
        BuildManager.TurretsBuilded.Add(gameObject);
    }

    private void OnDisable()
    {
        BuildManager.TurretsBuilded.Remove(gameObject);
    }

    public virtual void SetRange(float value)
    {
        if (value <= 0f) Debug.Log("Incorrect value to update range!");        
        else range = value;
    }

    public void SetCost(float value)
    {
        if (value <= 0f) Debug.Log("Incorrect value to update cost!");
        else cost = value;
    }

    public void SetPosition(Vector3 pos)
    {
        transform.position = pos;
    }

    public void SetStatus(int status)
    {
        // enum?
    }

    public void BuffRange(float value)
    {
        rangeBoost += value;
        SetRange(initialRange * rangeBoost);
    }

    public void SetRateBoost(float value)
    {
       rateBoost = value;
    }

    public void SetDamageBoost(float value)
    {
        damageBoost = value;
    }

    public void UpgradeTower()
    {
        if (!turretMaximized)
        {
            UpgradeStatus();
            numberOfUpgrades++;
            if (numberOfUpgrades < maxUpgrade) Debug.Log("Number of Upgrades:" + numberOfUpgrades);
            else
            {
                turretMaximized = true;
                Debug.Log("Number of Upgrades: Maximized");
            }
        }
    }
    
    protected abstract void UpgradeStatus();

    //public abstract void BuffTower();
}
