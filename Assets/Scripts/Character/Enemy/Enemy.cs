namespace GGJ2018.Character.Enemy
{
    using UnityEngine;
    using UnityEngine.AI;
    using Audio;
    using ObjectPooling;

    public abstract class Enemy : MonoBehaviour
    {
        public int maxHealth = 100;
        public Transform foot;
        public Transform gunPos;
        public EnemySFXManager sfx;
        public Animator enemyAnimator;
        public int damage;
        public bool agro;
        public bool shooting;
        public NavMeshAgent agent;
        public int Health { get; private set; }
        public GameObject ragdoll;
        public bool disableStepSonar = false;
      

        private void Start()
        {
          
            this.Health = maxHealth;
            this.agro = false;
            LocalInit();
        }

        private void Update()
        {
            if (Managers.GameState.Instance.CurrentState != Managers.GameState.State.Playing)
                return;

            LocalUpdate();
            if (this.Health <= 0)
            {
                //sfx.PlayEnemyDieSFX();
                GameObject temp = Instantiate(GGJ2018.Managers.PlayerManager.Instance.deathSound);
                temp.transform.position = transform.position;
                this.gameObject.SetActive(false);
                if (ragdoll != null)
                {
                    GameObject.Instantiate(ragdoll, transform.position, transform.rotation);
                }
            }
        }

        public int GetDamage()
        {
            return this.damage;
        }

        protected void Shoot()
        {
            GameObject g = BulletPool.Instance.GetBullet(BulletPool.BulletTypes.Enemy);
            if (g != null)
            {
                g.transform.position = gunPos.position;
                g.transform.rotation = this.transform.rotation;
                shooting = true;
                enemyAnimator.SetTrigger("fireGun");
                this.sfx.PlayEnemyGunfireSFX();
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag == "PlayerWeapon")
            {
                this.Health -= collision.gameObject.GetComponent<ObjectPooling.Bullets.Bullet>().GetDamage();
                sfx.PlayEnemyGetHitSFX();
                TookDamage();
                this.agro = true;
                this.InAgroRange();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                InAgroRange();
                this.agro = true;
                this.InAgroRange();
            }

            if (other.gameObject.tag == "PlayerWeapon")
            {
                ObjectPooling.Bullets.Bullet temp = other.gameObject.GetComponent<ObjectPooling.Bullets.Bullet>();
                if (temp == null)
                {
                    this.Health -= 1;
                }
                else
                {
                    this.Health -= temp.GetDamage();
                }
                sfx.PlayEnemyGetHitSFX();
                TookDamage();
            }
            if (other.gameObject.tag == "CaneWeaponHitBox" && other.GetComponent<BoxCollider>() != null)
            {
                other.GetComponent<BoxCollider>().enabled = false;
            }
        }

        public void StepEvent()
        {
            if (!disableStepSonar)
            {
                sfx.PlayEnemyStepSFX();
                GameObject g = SonarPool.Instance.GetSonar(1f, 1f);
                g.transform.position = foot.position;
            }
        }

        protected abstract void LocalInit();
        protected abstract void LocalUpdate();
        protected abstract void TookDamage();
        protected abstract void InAgroRange();
    }
}
