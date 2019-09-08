using UnityEngine;

public class SkillReveal : SkillGlobal
{
    public float duration = 10f;

    private void Start()
    {
        InvokeRepeating("RevealTargets", 0, 1f);
    }

    private void Update()
    {
        duration -= Time.deltaTime;

        if (duration <= 0f)
        {
            End();
        }
    }

    private void RevealTargets()
    {
        FindTargets();
        foreach (GameObject target in enemies)
        {
            target.GetComponent<EnemyBase>().UpdateInvisible(false);
        }
    }

    private void End()
    {
        // anything else to be done before removing the skill from play? remove visibility from current enemies?
        Destroy(gameObject);
    }
}
