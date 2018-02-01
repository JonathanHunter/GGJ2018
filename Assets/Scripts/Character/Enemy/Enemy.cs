namespace GGJ2018.Character.Enemy
{
    using UnityEngine;
    using UnityEngine.AI;
    using Audio;
    using ObjectPooling;
   
    

    public abstract class Enemy :  MonoBehaviour  
    {

        public int maxHealth;
        public int Health { get; private set; }
        public int destPoint = 0;
        public int damage;
        public Transform foot;
        public Transform gunPos;
        public EnemySFXManager sfx;
        public Animator enemyAnimator;
        public GameObject ragdoll;
        public NavMeshAgent agent;
        public bool agro;
        public bool shooting;
        public bool disableStepSonar = false;
        public bool smart;
        public float cooldown = 5f;

        private float fov = 90f;


        private void Start()
        {          
            this.Health = maxHealth;
            this.agro = false;
            this.smart = false;
            LocalInit();
            if(smart)
            {
               
            }
        }

        private void FixedUpdate()
        {

        }

        private void Update()
        {
            if (Managers.GameState.Instance.CurrentState != Managers.GameState.State.Playing)
            {
                if(agent != null)
                {
                    agent.enabled = false;
                }
                return;
            }

            if (!agent.enabled)
                agent.enabled = true;

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

        public void GotoNextPoint()
        {
            // Returns if no points have been set up
          
            // Set the agent to go to the currently selected destination.
            agent.destination =transform.position;

            // Choose the next point in the array as the destination,
            // cycling to the start if necessary.
           // destPoint = (destPoint + 1) % points.Length;
        }

        protected void Shoot()
        {
            GameObject g = BulletPool.Instance.GetBullet(BulletPool.BulletTypes.Enemy);

            if (g != null)
            {
                GGJ2018.ObjectPooling.Bullets.Bullet rf = g.GetComponent<GGJ2018.ObjectPooling.Bullets.Bullet>();
                MeleeWeaponTrail[] fb = rf.trails;
                if (fb != null)
                {
                    foreach (MeleeWeaponTrail m in fb)
                    {
                        m.startTrail();

                    }
                }
                g.transform.position = gunPos.position;
                g.transform.rotation = this.transform.rotation;
                shooting = true;
                enemyAnimator.SetTrigger("fireGun");
                this.sfx.PlayEnemyGunfireSFX();
            }
        }


        // fixed update on raycasts
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
                    this.Health -= 2;
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
            sfx.PlayEnemyStepSFX();
            if (!disableStepSonar)
            {
                GameObject g = SonarPool.Instance.GetSonar(1f, 1f);
                if(g != null)
                    g.transform.position = foot.position;
            }
        }

        protected abstract void LocalInit();
        protected abstract void LocalUpdate();
        protected abstract void TookDamage();
        protected abstract void InAgroRange();
    }
}
