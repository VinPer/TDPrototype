using UnityEngine;

public class TowerArea : TowerNonProjectile
{
    public string enemyTag = "Enemy";

    private void Start()
    {
        // logic to set scale of collision area
    }

    public override void SetRange(float value)
    {
        base.SetRange(value);
        // logic to increase scale of collision area
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.tag != enemyTag) return;

        Enemy e = col.GetComponent<Enemy>();
        e.ActivateDebuff(debuffIntensity, Mathf.Infinity, debuffElement);
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.tag != enemyTag) return;

        Enemy e = col.GetComponent<Enemy>();
        e.ActivateDebuff(debuffIntensity, debuffDuration, debuffElement);
    }

    public override void UpgradeTower()
    {
        // upgrade logic
    }
}
