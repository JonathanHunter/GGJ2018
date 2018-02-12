namespace GGJ2018.Util
{
    using UnityEngine;
    using Character.Enemy;

    public class BossSpawner : MonoBehaviour
    {
        public Enemy boss;
        public Effects.ExplodingDesk.ExplodingDesk desk;
        public GameObject bossDoor;
        public GameObject endTrigger;
        public Collider fightTrigger;

        private bool fightStarted;
        private bool doOnce;
        private bool spawnedEnd;

        private void Start()
        {
            this.fightStarted = false;
            this.doOnce = false;
            this.spawnedEnd = false;
        }

        private void Update()
        {
            if (this.fightStarted)
            {
                if(!this.doOnce && this.desk == null)
                {
                    this.doOnce = true;
                    this.boss.gameObject.SetActive(true);
                }
                else if (this.desk == null && this.boss.Health <= 0 && !this.spawnedEnd)
                {
                    GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
                    foreach (GameObject e in enemies)
                        e.SetActive(true);

                    this.endTrigger.SetActive(true);
                    this.spawnedEnd = true;
                }
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag == "Player" && !this.fightStarted)
            {
                this.fightStarted = true;
                this.desk.explode();
                this.fightTrigger.enabled = false;
                this.bossDoor.SetActive(true);
                GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
                foreach (GameObject e in enemies)
                    e.SetActive(false);
            }
        }
    }
}
