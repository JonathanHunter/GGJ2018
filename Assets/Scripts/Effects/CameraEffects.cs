namespace GGJ2018.Effects
{
    using UnityEngine;

    /// <summary>
    /// Juice effects for the player camera
    /// </summary>
    public class CameraEffects : MonoBehaviour
    {
        private float minDamage = 0.0f, maxDamage = 5.0f, damageTime = 0, damageMaxTime = 0.35f, minScreech = 0.0f, maxScreech = 0.5f, screechTime = 0, screechMaxTime = 0.6f;

        public FullScreenWhiteout whiteOut;

        void Update()
        {
            if (Managers.GameState.Instance.CurrentState != Managers.GameState.State.Playing)
                return;

            /*if (Input.GetKeyDown(KeyCode.Z)){
                TakeDamage();
            }
            if (Input.GetKeyDown(KeyCode.X)){
                Screech();
            }*/
            if (damageTime > 0)
            {
                transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, Mathf.Lerp(minDamage, maxDamage, damageTime/damageMaxTime));
                damageTime -= Time.deltaTime;
                if (damageTime <= 0)
                {
                    transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, 0);
                }
            }
            if (screechTime > 0)
            {
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, Mathf.Lerp(minScreech, maxScreech, screechTime / screechMaxTime));
                screechTime -= Time.deltaTime;
                if (screechTime <= 0)
                {
                    AbortScreech();
                }
            }
        }

        /// <summary>
        /// Rotates the camera sideways and flashes the screen red for taking damage
        /// </summary>
        public void TakeDamage()
        {
            maxDamage *= Random.Range(0.0f, 1.0f) < 0.5f ? 1 : -1;
            damageTime = damageMaxTime;
            whiteOut.ScreenFlash(new Color(1, 0, 0, 0.5f), damageMaxTime, 2);
            AbortScreech();
        }

        /// <summary>
        /// Jerks the camera forward and flashes the screen white for taking damage
        /// </summary>
        public void Screech()
        {
            screechTime = screechMaxTime;
            maxDamage *= Random.Range(0.0f, 1.0f) < 0.5f ? 1 : -1;
            damageTime = damageMaxTime;
            whiteOut.ScreenFlash(new Color(1, 1, 1, 0.5f), screechMaxTime, 1);
        }

        /// <summary>
        /// Aborts the screech screenflash if the player takes damage, as this is more important information
        /// </summary>
        private void AbortScreech()
        {
            screechTime = 0;
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 0);
        }
    }
}
