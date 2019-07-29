using UnityEngine;

public class SkillReveal : SkillGlobal
{
    public float duration = 10f;

    private void Update()
    {
        duration -= Time.deltaTime;

        if (duration <= 0f)
        {
            End();
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        EnemyBase enemy = col.GetComponent<EnemyBase>();
        enemy.UpdateInvisible(); // change this later
    }

    private void End()
    {
        // anything else to be done before removing the skill from play? remove visibility from current enemies?
        Destroy(gameObject);
    }
}
