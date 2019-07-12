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
        Enemy enemy = col.GetComponent<Enemy>();
        // logic to set enemy as revealed
        // enemy.SetInvisible(false);
    }

    private void End()
    {
        // anything else to be done before removing the skill from play? remove visibility from current enemies?
        Destroy(gameObject);
    }
}
