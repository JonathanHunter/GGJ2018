using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Script for handling the progression of cutscenes
/// </summary>
public class CutsceneHandler : MonoBehaviour
{

    public CutsceneObject[] cutsceneObjects;
    private int index = 0;

    // Update is called once per frame
    void Update()
    {
        if (cutsceneObjects[index].finished)
        {
            if (index < cutsceneObjects.Length)
            {
                index++;
            }
            else
            {
                //Go to next scene
            }
        }
        if (!cutsceneObjects[index].active)
        {
            cutsceneObjects[index].Activate();
        }
    }
}
