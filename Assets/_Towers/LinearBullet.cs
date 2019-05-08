using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinearBullet : Bullet
{
    protected Vector3 direction;
    public float decayTimer = 2f;

    public override void Seek(Transform _target)
    {
        direction = _target.position - transform.position;
        transform.LookAt(_target);
    }

    protected override void Update()
    {
        if (decayTimer < 0) Destroy(gameObject);

        transform.Translate(direction.normalized * speed * Time.deltaTime, Space.World);

        decayTimer -= Time.deltaTime;
    }

    public void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Enemy")
        {
            target = col.transform;
            HitTarget();
        }
    }
}
