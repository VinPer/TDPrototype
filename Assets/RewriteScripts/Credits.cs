using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Credits : MonoBehaviour
{
    public SceneFader sceneFader;
    public string sceneToGo = "MainMenu";

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKey)
        {
            sceneFader.FadeTo(sceneToGo);
        }
    }
}
