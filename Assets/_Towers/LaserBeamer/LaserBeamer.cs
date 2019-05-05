using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeamer : Turret
{
    public LineRenderer lineRenderer;
    public ParticleSystem impactEffect;
    public Light impactLight;
    public float damageOverTime = 5f;
    public float piercingValue = 0f;
    public float slowPercent = 50f;
    public float slowDuration = 5f;

    protected override void Update()
    {
        if (target == null)
        {
            if (lineRenderer.enabled)
            {
                lineRenderer.enabled = false;
                impactEffect.Stop();
                impactLight.enabled = false;
            }
            return;
        }

        LockOnTarget();
        Laser();
    }

    protected void Laser()
    {
        targetEnemy.TakeDamage(damageOverTime * Time.deltaTime, piercingValue);
        targetEnemy.ActivateDebuff(slowPercent, slowDuration, "slow");

        if (!lineRenderer.enabled)
        {
            lineRenderer.enabled = true;
            impactEffect.Play();
            impactLight.enabled = true;
        }

        lineRenderer.SetPosition(0, firePoint.position);
        lineRenderer.SetPosition(1, target.position);

        Vector3 dir = firePoint.position - target.position;

        impactEffect.transform.rotation = Quaternion.LookRotation(dir);
        impactEffect.transform.position = target.position + dir.normalized * .5f;
    }
}
