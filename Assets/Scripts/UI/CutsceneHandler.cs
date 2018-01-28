using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Script for handling the progression of cutscenes
/// </summary>
public class CutsceneHandler : MonoBehaviour
{

    public CutsceneObject[] cutsceneObjects;
    private int index = 0;

    bool inactive = false;

    // Update is called once per frame
    void Update()
    {
        if (cutsceneObjects[index].finished && !inactive)
        {
            if (index < cutsceneObjects.Length - 1)
            {
                index++;
            }
            else
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("Cavern");
                inactive = true;
            }
        }
        if (!cutsceneObjects[index].active && !inactive)
        {
            cutsceneObjects[index].Activate();
        }
    }
}
