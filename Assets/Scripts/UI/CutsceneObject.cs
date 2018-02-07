namespace GGJ2018.Cutscene
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
    using Util;

    public class CutsceneObject : MonoBehaviour
    {
        public AudioSource voiceClip;
        public Image image;
        public Text skipIcon;
        private float skipTimer, skipTimerMax = 2.0f;
        Color inactiveCol, activeCol;
        public bool active = false, finished = false;

        void Start()
        {
            inactiveCol = new Color(image.color.r, image.color.g, image.color.b, 0);
            activeCol = new Color(image.color.r, image.color.g, image.color.b, 1);
        }
		
        public void Activate()
        {
            skipIcon.enabled = false;
            active = true;
            voiceClip.Play();
            StartCoroutine(FadeInImage(0.75f));
        }

        IEnumerator FadeInImage(float duration)
        {
            for (float d = duration; d > 0; d -= Time.deltaTime)
            {
                image.color = Color.Lerp(activeCol, inactiveCol, d / duration);
                yield return new WaitForSeconds(0);
            }
            image.color = activeCol;
            yield return new WaitForSeconds(voiceClip.clip.length - 1.25f);
            StartCoroutine(FadeOutImage(0.75f));
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
            skipIcon.enabled = false;
        }

        void Update()
        {
            if (active && !finished && (CustomInput.BoolFreshPress(CustomInput.UserInput.Accept) || CustomInput.BoolFreshPress(CustomInput.UserInput.Melee)
                || CustomInput.BoolFreshPress(CustomInput.UserInput.Shoot) || CustomInput.BoolFreshPress(CustomInput.UserInput.Screech) || CustomInput.BoolFreshPress(CustomInput.UserInput.Pause)))
            {
                if (!skipIcon.enabled)
                {
                    skipIcon.enabled = true;
                    skipTimer = skipTimerMax;
                }
                else
                {
                    Skip();
                }
            }
            if (skipTimer > 0)
            {
                skipTimer -= Time.deltaTime;
                if (skipTimer <= 0)
                {
                    skipIcon.enabled = false;
                }
            }
        }
		
        void Skip()
        {
            StopCoroutine("FadeInImage");
            StopCoroutine("FadeOutImage");
            voiceClip.Stop();
            image.color = inactiveCol;
            image.enabled = false;
            active = false;
            finished = true;
            skipIcon.enabled = false;
        }
    }
}
