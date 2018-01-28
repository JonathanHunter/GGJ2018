namespace GGJ2018.Util
{
    using UnityEngine;
    using Character.Enemy;

    public class BossSpawner : MonoBehaviour
    {
        public Enemy boss;
        public GameObject bossDoor;
        public GameObject endTrigger;

        private bool fightStarted;

        private void Start()
        {
            this.fightStarted = false;
        }

        private void Update()
        {
            if(this.fightStarted)
            {
                if(this.boss.Health <= 0)
                {
                    this.endTrigger.SetActive(true);
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.tag == "Player" && !this.fightStarted)
            {
                this.fightStarted = true;
                this.boss.gameObject.SetActive(true);
            }
        }
    }
}
