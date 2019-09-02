using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Freeze : MonoBehaviour
{
    public float duration = 5f;
    public float freezeTime = 5f;
    private float initialFreezeTime;
    public float piercing = 0f;
    public GameObject freezeEffect;
    private GameObject freezerEffect;
    private Enums.Element ice = Enums.Element.ice;
    public TowerFreezer freezer;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 pos = new Vector3(transform.position.x, transform.position.y, transform.position.z - 1);
        freezerEffect = Instantiate(freezeEffect, transform);
        freezerEffect.transform.position = pos;
        initialFreezeTime = freezeTime;
    }
    
    void Update()
    {
        if(freezer.GetTarget() != null && GetComponent<CapsuleCollider>().enabled) freezerEffect.GetComponent<ParticleSystem>().Play();
        else freezerEffect.GetComponent<ParticleSystem>().Stop();
        if (freezer.GetTarget() == null && freezeTime < initialFreezeTime) StartCoroutine(Reload(3f));

    }
    
    private void OnTriggerStay(Collider col)
    {
        if (col.tag != "Enemy") return;
        if (freezeTime > 0)
        {
            FreezeTarget(col.transform);
            freezeTime -= Time.deltaTime;
        }
        else
        {
            EnemyBase e = col.GetComponent<EnemyBase>();
            e.freezeStatus = 0;
            StartCoroutine(Reload(3f));
        }
    }

    private void FreezeTarget(Transform enemy)
    {
        EnemyBase e = enemy.GetComponent<EnemyBase>();
        e.TakeDamage(5 * e.freezeStatus * Time.deltaTime,piercing,ice);
        e.ActivateDebuff(100 * e.freezeStatus, duration, ice);
        if (e.freezeStatus > 1) e.freezeStatus = 1;
        else e.freezeStatus += .01f;
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.tag != "Enemy") return;
        EnemyBase e = col.GetComponent<EnemyBase>();
        e.freezeStatus = 0;
    }

    private IEnumerator Reload(float time)
    {
        freezerEffect.GetComponent<ParticleSystem>().Stop();
        GetComponent<CapsuleCollider>().enabled = false;
        freezeTime = initialFreezeTime;
        yield return new WaitForSeconds(time);
        GetComponent<CapsuleCollider>().enabled = true;
    }
}
