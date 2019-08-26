using System.Collections;
using UnityEngine;

public class Effect 
{
    public static IEnumerator PlayEffect(GameObject effect, Transform transform)
    {
        effect.transform.position = transform.position;
        effect.transform.rotation = transform.rotation;
        effect.SetActive(true);
        yield return new WaitForSeconds(2f);
        effect.SetActive(false);
    }
}
