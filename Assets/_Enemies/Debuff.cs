using UnityEngine;

public class Debuff
{
    public float duration;
    public float level;
    public bool isActive;
    public bool accumulates;

    public Debuff (bool accumulates)
    {
        duration = 0;
        level = 0;
        isActive = false;
        this.accumulates = accumulates;
    }

    public void Refresh(float lvl, float dur)
    {
        if (accumulates) level += lvl;
        else level = lvl;
        duration = dur;
    }

    public void Zero()
    {
        duration = 0;
        level = 0;
        isActive = false;
    }
}
