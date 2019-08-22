using UnityEngine;

public class TowerArea : TowerNonProjectile
{
    public string enemyTag = "Enemy";

    protected void Start()
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

        EnemyBase e = col.GetComponent<EnemyBase>();
        e.ActivateDebuff(debuffIntensity, Mathf.Infinity, element);
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.tag != enemyTag) return;

        EnemyBase e = col.GetComponent<EnemyBase>();
        e.ActivateDebuff(debuffIntensity, debuffDuration, element);
    }

    protected override void UpgradeStatus()
    {
        range += rangeUpgrade;
    }

}
