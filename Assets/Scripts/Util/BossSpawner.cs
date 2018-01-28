namespace GGJ2018.Util
{
    using UnityEngine;
    using Character.Enemy;

    public class BossSpawner : MonoBehaviour
    {
        public Enemy boss;
        public GameObject bossDoor;
        public GameObject endTrigger;
        public Collider fightTrigger;

        private bool fightStarted;

        private void Start()
        {
            this.fightStarted = false;
        }

        private void Update()
        {
            if (this.fightStarted)
            {
                if (this.boss.Health <= 0)
                {
                    this.endTrigger.SetActive(true);
                }
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag == "Player" && !this.fightStarted)
            {
                this.fightStarted = true;
                this.boss.gameObject.SetActive(true);
                this.fightTrigger.enabled = false;
                this.bossDoor.SetActive(true);
            }
        }
    }
}
