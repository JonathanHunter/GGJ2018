namespace GGJ2018.UI
{
    using UnityEngine;
    using UnityEngine.UI;

    public class HealthUI : MonoBehaviour
    {

        public Image[] hearts, emptyHearts;
        private int maxHearts = 3, currentHearts = 3, prevHearts = 3;
        private float bigPulseTimer = 0, maxBigPulseTimer = 0.25f;

        Vector3 initScale;

        void Start()
        {
            initScale = transform.localScale;
        }

        /// <summary>
        /// Call this at the start of the scene, and if the player's max health ever changes
        /// </summary>
        /// <param name="maxHealth"></param>
        public void SetMaxHealth(int maxHearts)
        {
            this.maxHearts = maxHearts;
            currentHearts = maxHearts;
            prevHearts = maxHearts;
        }

        /// <summary>
        /// Use this to update the UI if the player takes damage
        /// </summary>
        /// <param name="damage"></param>
        public void TakeDamage(int damage)
        {
            currentHearts = Mathf.Max(currentHearts - damage, 0);
            UpdateUI();
        }

        /// <summary>
        /// Use this to update the UI if the player regains some health
        /// </summary>
        /// <param name="points"></param>
        public void Heal(int points)
        {
            currentHearts = Mathf.Min(currentHearts + points, maxHearts);
            UpdateUI();
        }

        void UpdateUI()
        {
            for (int i = 0; i < hearts.Length; i++)
            {
                if (i >= currentHearts)
                {
                    emptyHearts[i].enabled = true;
                    hearts[i].enabled = false;
                }
                else
                {
                    emptyHearts[i].enabled = false;
                    hearts[i].enabled = true;
                }
            }
            if (prevHearts != currentHearts)
            {
                bigPulseTimer = maxBigPulseTimer;
            }
            prevHearts = currentHearts;
        }

        void Update()
        {
            /*if (Input.GetKeyDown(KeyCode.Q)){
                TakeDamage(1);
            }
            if (Input.GetKeyDown(KeyCode.E)){
                Heal(1);
            }*/

            if (bigPulseTimer > 0)
            {
                transform.localScale = Vector3.Lerp(initScale, initScale * 1.25f, bigPulseTimer / maxBigPulseTimer);
                bigPulseTimer -= Time.deltaTime;
                if (bigPulseTimer <= 0)
                {
                    transform.localScale = initScale;
                }
            }
        }
    }
}
