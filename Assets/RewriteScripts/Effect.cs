using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect 
{
    public static IEnumerator PlayEffect(GameObject effect)
    {
        effect.SetActive(true);
        yield return new WaitForSeconds(2f);
        effect.SetActive(false);
    }
}
