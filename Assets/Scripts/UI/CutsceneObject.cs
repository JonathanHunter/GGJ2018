﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CutsceneObject : MonoBehaviour {

    public AudioSource voiceClip;
    public Image image;
    Color inactiveCol, activeCol;
    public bool active = false, finished = false;
    bool audioActive = false;

    void Start()
    {
        inactiveCol = new Color (image.color.r, image.color.g, image.color.b, 0);
        activeCol = new Color(image.color.r, image.color.g, image.color.b, 1);
    }

    public void Activate()
    {
        active = true;
        audioActive = true;
        voiceClip.Play();
        StartCoroutine(FadeInImage(0.75f));
    }

    void Update()
    {
        if (audioActive && !voiceClip.isPlaying)
        {
            audioActive = false;
            StartCoroutine(FadeOutImage(0.75f));
        }
    }

    IEnumerator FadeInImage(float duration)
    {
        for (float d = duration; d > 0; d -= Time.deltaTime)
        {
            image.color = Color.Lerp(activeCol, inactiveCol, d/duration);
            yield return new WaitForSeconds(0);
        }
        image.color = activeCol;
    }

    IEnumerator FadeOutImage(float duration)
    {
        for (float d = duration; d > 0; d -= Time.deltaTime)
        {
            image.color = Color.Lerp(inactiveCol, activeCol, d / duration);
            yield return new WaitForSeconds(0);
        }
        image.color = inactiveCol;
        active = false;
        finished = true;
    }
}
