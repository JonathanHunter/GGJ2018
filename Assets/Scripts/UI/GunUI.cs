namespace GGJ2018.UI
{
    using System.Collections;
    using UnityEngine;
    using UnityEngine.UI;

    public class GunUI : MonoBehaviour
    {
        public AudioSource[] ReloadSounds;
        private int bulletsInClip = 6, maxClipSize = 6;
        private float reloadCounter = 0, maxReloadCounter = 14, toggleFlashTimer, maxToggleFlashTimer = 0.15f;
        bool flashToggled = false;

        public Image[] bullets, emptyBullets, flashBullets;

        Vector3 initScaleBullet, initScale;
        float initPosYBullet;

        private AudioSource sfx;

        /// <summary>
        /// Whether or not the Reload is recharging
        /// </summary>
        public bool IsReloading { get; private set; }

        void Start()
        {
            sfx = GetComponent<AudioSource>();
            initScale = transform.localScale;
            initScaleBullet = emptyBullets[0].transform.localScale;
            initPosYBullet = emptyBullets[0].transform.localPosition.y;
        }

        /// <summary>
        /// Call this to kick off the reload presentation
        /// </summary>
        /// <param name="duration"></param>
        /// How long the reload will take
        public void StartReload()
        {
            reloadCounter = 13;
            bulletsInClip = 0;
            toggleFlashTimer = maxToggleFlashTimer;
            IsReloading = true;
            flashToggled = false;
        }

        /// <summary>
        /// Used to update UI values
        /// </summary>
        /// <param name="bulletsInClip"></param>
        /// Number of bullets in your clip
        public void Fire(int bulletsInClip)
        {
            if (bulletsInClip >= 0)
            {
                this.bulletsInClip = bulletsInClip;
                UseBullet(bulletsInClip + 1);
            }
        }

        void Update()
        {
            if (Managers.GameState.Instance.CurrentState != Managers.GameState.State.Playing)
                return;
            /*if (Input.GetKeyDown(KeyCode.Q))
            {
                Fire(bulletsInClip - 1);
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                StartReload();
            }*/

            if (reloadCounter > 0)
            {
                if (reloadCounter == 1)
                {
                    reloadCounter = 0;
                    IsReloading = false;
                    toggleFlashTimer = 0;
                    flashToggled = false;
                    ResetBullets();
                }
                else
                {
                    toggleFlashTimer -= Time.deltaTime;
                    if (toggleFlashTimer <= 0)
                    {
                        ToggleFlash(flashToggled, bulletsInClip);
                        flashToggled = !flashToggled;
                        if (flashToggled == false)
                        {
                            bulletsInClip++;
                            if (reloadCounter > 2) {
                                ReloadSounds[Random.Range(0, ReloadSounds.Length)].Play();
                            }
                        }
                        toggleFlashTimer = maxToggleFlashTimer;
                        reloadCounter -= 1;
                    }
                }
            }
        }

        void ToggleFlash(bool flashToggled, int bulletIndex)
        {
            for (int i = 0; i < flashBullets.Length; i++)
            {
                if (i <= bulletIndex)
                {
                    flashBullets[i].enabled = flashToggled;
                    emptyBullets[i].enabled = !flashToggled;
                }
                else
                {
                    emptyBullets[i].enabled = true;
                }
            }
        }

        void UseBullet(int index)
        {
            bullets[index - 1].enabled = false;
            emptyBullets[index - 1].enabled = true;
            StartCoroutine(BulletJuice(index - 1, 0.1f));
        }

        void ResetBullets()
        {
            for (int i = 0; i < bullets.Length; i++)
            {
                bullets[i].enabled = true;
                flashBullets[i].enabled = false;
                emptyBullets[i].enabled = false;
            }
            sfx.Play();
            StartCoroutine(ScaleEverything(0.25f));
        }

        IEnumerator BulletJuice(int index, float duration)
        {
            for (float d = duration; d > 0; d -= Time.deltaTime)
            {
                emptyBullets[index].transform.localScale = Vector3.Lerp(initScaleBullet, new Vector3(initScaleBullet.x * 1.01f, initScaleBullet.y * 1.1f, initScaleBullet.z), d / duration);
                emptyBullets[index].transform.localPosition = Vector3.Lerp(new Vector3(emptyBullets[index].transform.localPosition.x, initPosYBullet, emptyBullets[index].transform.localPosition.z),
                    new Vector3(emptyBullets[index].transform.localPosition.x, initPosYBullet * 1.025f, emptyBullets[index].transform.localPosition.z), d / duration);
                yield return new WaitForSeconds(0);
            }
            emptyBullets[index].transform.localScale = initScaleBullet;
            emptyBullets[index].transform.localPosition = new Vector3(emptyBullets[index].transform.localPosition.x, initPosYBullet, emptyBullets[index].transform.localPosition.z);
        }

        IEnumerator ScaleEverything(float duration)
        {
            for (float d = duration; d > 0; d -= Time.deltaTime)
            {
                transform.localScale = Vector3.Lerp(initScale, initScale * 1.025f, d / duration);
                yield return new WaitForSeconds(0);
            }
            transform.localScale = initScale;
        }
    }
}