namespace GGJ2018.Scripts.Effects.ExplodingDesk
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
                    Destroy(desk);
                    explodedDesk.SetActive(true);
                    explodedDesk.transform.DetachChildren();
                    Destroy(explodedDesk);
                    sfPlayed = true;
                    myAudio.PlaySong(0);
                }
            }

            if (sfPlayed){
                soundTimer -= Time.deltaTime;
                if(soundTimer <= 0)
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