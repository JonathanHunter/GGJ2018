namespace GGJ2018.Effects.ExplodingDesk
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using Util;

    public class ExplodingDesk : MonoBehaviour
    {

        public GameObject desk;
        public GameObject explodedDesk;
        public SoundPlayer myAudio;
        public bool isComplete = false;
        private bool sfPlayed = false;

        private float soundTimer = 3f;

        public float blastTimer = 1f;
        public int sonarBlastCount = 13;

        // Use this for initialization
        void Start()
        {
            desk.SetActive(true);
            explodedDesk.SetActive(false);
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
                    GameObject g = ObjectPooling.SonarPool.Instance.GetSonar(3f, 6f);
                    if (g != null)
                    {
                        g.transform.position = this.transform.position;
                    }
                    Destroy(desk);
                    explodedDesk.SetActive(true);
                    explodedDesk.transform.DetachChildren();
                    Destroy(explodedDesk);
                    sfPlayed = true;
                    myAudio.PlaySong(0);

                    for (int i = 0; i < sonarBlastCount; i++)
                    {
                        float tempTime = blastTimer;
                        while(blastTimer > 0)
                        {
                            blastTimer -= Time.deltaTime;
                        }
                        float range = Random.Range(2, 5);
                        g.transform.position = new Vector3(range, range);
                        blastTimer = tempTime;
                    }
                }
            }

            if (sfPlayed){
                soundTimer -= Time.deltaTime;
                if (soundTimer <= 0)
                {
                    Destroy(gameObject);
                }
            }
        }

        public void explode()
        {
            isComplete = true;
        }
    }
}