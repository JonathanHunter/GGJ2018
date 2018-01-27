namespace GGJ2018.Character.Enemy
{
    using UnityEngine;
    using Audio;
    using ObjectPooling;

    public abstract class Enemy : MonoBehaviour
    {
        public int maxHealth = 100;
        public Transform foot;
        public EnemySFXManager sfx;
        public int damage;
        public bool agro;
        public int Health { get; private set; }

        private void Start()
        {
            this.Health = maxHealth;
            LocalInit();
        }

        private void Update()
        {
            if (Managers.GameState.Instance.CurrentState != Managers.GameState.State.Playing)
                return;

            LocalUpdate();
            if (this.Health <= 0)
            {
                sfx.PlayEnemyDieSFX();
                this.gameObject.SetActive(false);
            }
        }

        public int GetDamage()
        {
            return this.damage;
        }

        protected void Shoot()
        {


        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag == "PlayerWeapon")
            {
                this.Health -= collision.gameObject.GetComponent<ObjectPooling.Bullets.Bullet>().GetDamage();
                sfx.PlayEnemyGetHitSFX();
                TookDamage();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                InAgroRange();
            }

            if (other.gameObject.tag == "PlayerWeapon")
            {
                this.Health -= other.gameObject.GetComponent<ObjectPooling.Bullets.Bullet>().GetDamage();
                sfx.PlayEnemyGetHitSFX();
                TookDamage();
            }
        }

        public void StepEvent()
        {
            sfx.PlayEnemyStepSFX();
            GameObject g = SonarPool.Instance.GetSonar(1f, 1f);
            g.transform.position = foot.position;
        }

        protected abstract void LocalInit();
        protected abstract void LocalUpdate();
        protected abstract void TookDamage();
        protected abstract void InAgroRange();
    }
}
