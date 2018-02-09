namespace GGJ2018.Character.Enemy
{
    using UnityEngine;
    using UnityEngine.AI;
    using Audio;
    using ObjectPooling;

    /*
     * LEVEL 1 -  Shoot until dead , no chase
     * LEVEL 2 -  Shoot & Chase, stops if you lose LOS
     * LEVEL 3 -  Shoot, Chase, & Hunts when out of LOS
     * LEVEL 4 -  Shoot, Chase,Hunt, Dodge
     * LEVEL 5 -  Shoot, Chase, Hunt, Dodge, Flee
     * 
     */


    public abstract class Enemy : MonoBehaviour
    {
        public int level = 1;
        public int maxHealth;
        public int Health { get; private set; }
        public int destPoint = 0;
        public int damage;
        public Transform foot;
        public Transform gunPos;
        public Transform agroBubble;
        public EnemySFXManager sfx;
        public Animator enemyAnimator;
        public GameObject ragdoll;
        public NavMeshAgent agent;
        public bool los = false; // line of sight
        public bool agro;
        public bool shooting;
        public bool disableStepSonar = false;
        public static float animRatio = 0.80f;
        public float cooldown;
        public float agroRange = 3f;
        public float fov = 90f;
        public float interest = 0;
        private float hate = 15f;
        private bool smart = true;
 
        private void Start()
        {
            agroBubble = this.transform;
            this.Health = maxHealth;
            this.agro = false;
            agent.speed = 1f;
            interest = hate - cooldown;
            LocalInit();

            switch (level)
            {
                case 0:
                    cooldown = 8f;
                    fov = 60f;
                    agent.speed += 0.25f;
                    agroRange = 10f;
                    break;
                case 1:
                    cooldown = 7f;
                    fov = 70f;
                    agent.speed += 0.5f;
                    agroRange = 8f;
                    break;
                case 2:
                    cooldown = 6f;
                    fov = 80f;
                    agent.speed += 0.75f;
                    agroRange = 6f;
                    break;
                case 3:
                    cooldown = 5f;
                    fov = 90f;
                    agent.speed += 1f;
                    agroRange = 5f;
                    break;
                case 4:
                    cooldown = 4f;
                    fov = 100f;
                    agent.speed += 1.25f;
                    agroRange = 4f;
                    break;
                case 5:
                    cooldown = 3f;
                    fov = 110f;
                    agent.speed += 1.5f;
                    agroRange = 3f;
                    break;
            }
        }

        private void Update()
        {
            if (Managers.GameState.Instance.CurrentState != Managers.GameState.State.Playing)
            {
                if (agent != null)
                {
                    agent.enabled = false;
                }
                return;
            }
            else if (agent != null)
            {
                agent.enabled = true;
            }

            LocalUpdate();

            if (this.Health <= 0)
            {
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

        public virtual void GotoNextPoint()
        { 
            // Set the agent to go to the currently selected destination.
            agent.destination = transform.position;
        }

        public virtual void  TargetPlayer()
        {
            //override
        }

        public virtual void Chase()
        {
            //override
        }

        ///Shoots 
        public void Shoot()
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
            if (other.gameObject.tag == "PlayerWeapon") // if hit by player bullet
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
            if (other.gameObject.tag == "CaneWeaponHitBox" && other.GetComponent<BoxCollider>() != null) //hit by player cane
            {
                other.GetComponent<BoxCollider>().enabled = false;
            }
        }

        //Enemy Steps and sound generation
        public void StepEvent()
        {
            sfx.PlayEnemyStepSFX();
            if (!disableStepSonar)
            {
                GameObject g = SonarPool.Instance.GetSonar(1f, 1f);
                if (g != null)
                    g.transform.position = foot.position;
            }
        }

        protected abstract void LocalInit();
        protected abstract void LocalUpdate();
        protected abstract void TookDamage();
        protected abstract void InAgroRange();
    }
}
