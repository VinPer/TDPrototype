using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinearBullet : Bullet
{
    protected Vector3 direction;
    [HideInInspector]
    public float initialDecayTimer;
    public float decayTimer = 2f;

    private void Start()
    {
        initialDecayTimer = decayTimer;
    }

    public override void Seek(Transform _target)
    {
        direction = _target.position - transform.position;
        transform.LookAt(_target);
    }

    public void Seek(Vector3 dir)
    {
        direction = dir;
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
