namespace GGJ2018.UI
{
    using UnityEngine;
    using UnityEngine.UI;

    public class ScreechUI : MonoBehaviour
    {

        public Image[] screeches;

        private int reloadStatus = 0;
        private int flashStatus = 0;
        private float reloadTimer = 0, reloadTimerMax = 0.4f;

        private float growTimer = 0, maxGrowTimer = 0.15f;
        private bool isGrowing = true;

        private Vector3 initScale, initRot;

        /// <summary>
        /// Whether or not the Reload is recharging
        /// </summary>
        public bool IsRecharging { get; private set; }

        void Start()
        {
            initScale = transform.localScale;
            initRot = transform.eulerAngles;
        }

        /// <summary>
        /// Call this to kick off the screech recharge presentation
        /// </summary>
        /// <param name="duration"></param>
        /// How long the reload will take
        public void StartRecharge()
        {
            reloadTimer = reloadTimerMax;
            flashStatus = 0;
            reloadStatus = 0;
            IsRecharging = true;
            growTimer = 0;
            isGrowing = false;
            transform.localScale = initScale;
        }

        void Update()
        {
            /*if (Input.GetKeyDown(KeyCode.Q)){
                StartRecharge();
            }*/
            if (IsRecharging && reloadTimer > 0)
            {
                reloadTimer -= Time.deltaTime;
                if (reloadTimer <= 0)
                {
                    if (flashStatus < 4)
                    {
                        flashStatus++;
                        reloadTimer = reloadTimerMax;
                        UpdateUI();
                    }
                    else
                    {
                        flashStatus = 0;
                        if (reloadStatus < 2)
                        {
                            reloadStatus++;
                            reloadTimer = reloadTimerMax;
                            UpdateUI();
                        }
                        else
                        {
                            IsRecharging = false;
                            screeches[0].enabled = false;
                            screeches[1].enabled = false;
                            screeches[2].enabled = false;
                            screeches[3].enabled = true;
                            growTimer = maxGrowTimer;
                            isGrowing = true;
                        }
                    }
                }
            }
            if (growTimer > 0)
            {
                if (isGrowing)
                {
                    transform.localScale = new Vector3(Mathf.Lerp(initScale.x * 1.25f, initScale.x, growTimer / maxGrowTimer), Mathf.Lerp(initScale.y * 1.25f, initScale.y, growTimer / maxGrowTimer), initScale.z);
                }
                else
                {
                    transform.localScale = new Vector3(Mathf.Lerp(initScale.x, initScale.x * 1.25f, growTimer / maxGrowTimer), Mathf.Lerp(initScale.y, initScale.y * 1.25f, growTimer / maxGrowTimer), initScale.z);
                }
                growTimer -= Time.deltaTime;
                if (growTimer <= 0)
                {
                    if (isGrowing)
                    {
                        isGrowing = false;
                        growTimer = maxGrowTimer;
                    }
                    else
                    {
                        transform.localScale = initScale;
                    }
                }
            }
        }

        /// <summary>
        /// Updates the UI based on our flash status and our reload status
        /// </summary>
        void UpdateUI()
        {
            for (int i = 0; i < screeches.Length; i++)
            {
                if (i == reloadStatus)
                {
                    if (flashStatus % 2 == 1)
                    {
                        screeches[i].enabled = true;
                    }
                    else
                    {
                        screeches[i].enabled = false;
                    }
                }
                else
                {
                    screeches[i].enabled = false;
                }
            }
        }
    }
}
