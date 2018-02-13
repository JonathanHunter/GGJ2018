namespace GGJ2018.Effects.ExplodingDesk
{
    using UnityEngine;
    using Util;

    public class ExplodingDesk : MonoBehaviour
    {
        public GameObject desk;
        public GameObject explodedDesk;
        public SoundPlayer myAudio;
        public AudioSource ambientExitSound;
        public bool isComplete = false;
        public float explosionTime = .1f;
        public float soundTimer = .5f;

        private bool sfPlayed = false;
        private float explosionTimer;
        private bool flashed;
        private bool hasBoom = false;

        // Use this for initialization
        void Start()
        {
            desk.SetActive(true);
            explodedDesk.SetActive(false);
            this.flashed = false;
        }

        // Update is called once per frame
        void Update()
        {
            if (isComplete == false)
            {
                desk.SetActive(true);
                explodedDesk.SetActive(false);
            }
            else
            {
                if (sfPlayed == false)
                {
                    Destroy(desk);
                    explodedDesk.SetActive(true);
                    explodedDesk.transform.DetachChildren();
                    Destroy(explodedDesk);
                    sfPlayed = true;
                    myAudio.PlaySong(0);
                    if (ambientExitSound != null)
                    {
                        ambientExitSound.Stop();
                    }
                }
            }

            if (sfPlayed && !hasBoom)
            {
                if(!this.flashed)
                {
                    Managers.PlayerManager.Instance.player.GetComponentInChildren<CameraEffects>().Screech();
                    this.flashed = true;
                }

                if ((this.explosionTimer -= Time.deltaTime) <= 0)
                {
                    GameObject g = ObjectPooling.SonarPool.Instance.GetSonar(6f, 6f);
                    if (g != null)
                    {
                        g.transform.position = new Vector3(
                            this.transform.position.x - Random.Range(-1f, 1f),
                            0f,
                            this.transform.position.z - Random.Range(-1f, 1f));
                    }

                    this.explosionTimer = this.explosionTime;
                }

                soundTimer -= Time.deltaTime;
                if (soundTimer <= 0)
                {
                    hasBoom = true;
                    Destroy(gameObject, 1.5f);
                }
            }
        }

        public void explode()
        {
            isComplete = true;
        }
    }
}
