using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CutsceneObject : MonoBehaviour {

    public AudioSource voiceClip;
    public Image image;
    Color inactiveCol, activeCol;
    bool active = false;

    void Start()
    {
        inactiveCol = new Color (image.color.r, image.color.g, image.color.b, 0);
        activeCol = new Color(image.color.r, image.color.g, image.color.b, 1);
    }

    public void Activate()
    {
        active = true;
        voiceClip.Play();
        
    }

    IEnumerator FadeInImage(float duration)
    {
        for (float d = duration; d > 0; d -= Time.deltaTime)
        {
            image.color = Color.Lerp(activeCol, inactiveCol, d/duration);
        }
        yield return new WaitForSeconds(0);
    }
}
