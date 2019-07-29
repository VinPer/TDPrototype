using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyElemental : EnemyBase
{
    [Header("Element")]
    public Enums.Element element = Enums.Element.none;
    public float resistance = 0;

    public override void ActivateDebuff(float multiplier, float duration, Enums.Element debuffType)
    {
        if(debuffType == element)
        {
            return;
        }
        base.ActivateDebuff(multiplier,duration,debuffType);
    }

    public override void TakeDamage(float amount, float piercingValue, Enums.Element turretElement)
    {
        if(element == turretElement)
        {
            amount *= resistance;
        }
        base.TakeDamage(amount, piercingValue, turretElement);
    }
}
