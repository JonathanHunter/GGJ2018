namespace GGJ2018.Effects
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    /// Flash the full screen a certain color for a duration, will gradually fade away
    /// </summary>
    public class FullScreenWhiteout : MonoBehaviour
    {
        private int currentPriority = 0;
        private float duration, maxDuration;
        private Color flashCol;

        public Image uiImage;

        /// <summary>
        /// Flashes the screen the provided color, fading away over time of duration. Priority dictates if the screen flash outprioritizes another screen
        /// </summary>
        /// <param name="col"></param>
        /// The color of the flash (including the initial alpha)
        /// <param name="duration"></param>
        /// How long the flash takes to fade away
        /// <param name="priority"></param>
        /// The priority of the flash. If it is less than or equal to the current flash priority, it will restart it
        public void ScreenFlash(Color col, float duration, int priority = 1)
        {
            if (priority >= currentPriority)
            {
                currentPriority = priority;
                flashCol = col;
                this.duration = duration;
                maxDuration = duration;
            }
        }

        void Update()
        {
            if (duration > 0)
            {
                uiImage.color = new Color(flashCol.r, flashCol.g, flashCol.b, Mathf.Lerp(0, flashCol.a, duration/maxDuration));
                duration -= Time.deltaTime;
                if (duration <= 0)
                {
                    uiImage.color = new Color(flashCol.r, flashCol.g, flashCol.b, 0);
                    currentPriority = 0;
                }
            }
        }
    }
}
