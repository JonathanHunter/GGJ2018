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

    public CutsceneObject[] cutsceneObjects1, cutsceneObjects2, cutsceneObjects3, cutsceneObjects4;
    public Text credits;
    public int cutsceneVersion = 1;

    private CutsceneObject[] curObj;
    private int index = 0;

    bool inactive = false;

    void Start()
    {
        switch (cutsceneVersion) {
            case 1:
                curObj = cutsceneObjects1;
                break;
            case 2:
                curObj = cutsceneObjects2;
                break;
            case 3:
                curObj = cutsceneObjects3;
                break;
            case 4:
                curObj = cutsceneObjects4;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (curObj[index].finished && !inactive)
        {
            if (index < curObj.Length - 1)
            {
                index++;
            }
            else
            {
                switch (cutsceneVersion)
                {
                    case 1:
                        UnityEngine.SceneManagement.SceneManager.LoadScene("Cavern"); ;
                        break;
                    case 2:
                        UnityEngine.SceneManagement.SceneManager.LoadScene("Room");
                        break;
                    case 3:
                        UnityEngine.SceneManagement.SceneManager.LoadScene("CityHall");
                        break;
                    case 4:
                        StartCoroutine(ShowCredits());
                        break;
                }
                inactive = true;
            }
        }
        if (!curObj[index].active && !inactive)
        {
            curObj[index].Activate();
        }
    }

    IEnumerator ShowCredits()
    {
        yield return new WaitForSeconds(1.0f);
        float dur = 2.0f;
        Color initCol = credits.color, finalCol = new Color(credits.color.r, credits.color.g, credits.color.b, 1);
        for (float d = dur; d > 0; d -= Time.deltaTime)
        {
            credits.color = Color.Lerp(finalCol, initCol, d / dur);
            yield return new WaitForSeconds(0f);
        }
        credits.color = finalCol;
        yield return new WaitForSeconds(4.0f);
        for (float d = dur; d > 0; d -= Time.deltaTime)
        {
            credits.color = Color.Lerp(initCol, finalCol, d / dur);
            yield return new WaitForSeconds(0f);
        }
        credits.color = initCol;
        yield return new WaitForSeconds(1.0f);
        UnityEngine.SceneManagement.SceneManager.LoadScene("Title");
    }
}
