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
    

    public abstract class Enemy :  MonoBehaviour  
    {
        public int level = 0;
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
        public bool agro;
        public bool shooting;
        public bool disableStepSonar = false;
        public bool smart;
        public static float animRatio = 1.0f;
        public float cooldown = 5f;
        public float agroRange = 10f;
        private float fov = 90f;


        private void Start()
        {
            agroBubble = this.transform;
            this.Health = maxHealth;
            this.agro = false;
            LocalInit();
        }

        private void FixedUpdate()
        {

        }

        private void Update()
        {
            enemyAnimator.speed = agent.speed * animRatio; // animation speed reflects agent speed

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

            if(agent.remainingDistance >0.01)
            {
                enemyAnimator.SetBool("isMoving", true);

            }
            else
            {
                enemyAnimator.SetBool("isMoving", false);
            }

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

        public void TargetPlayer()
        {
            //override
        }
        
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

            //Depricated Moving to On Trigger Stay with Raycast
            /*
            if (other.gameObject.tag == "Player")
            {
                InAgroRange();
                this.agro = true;
                this.InAgroRange();
            }
            */
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
        

        void OnTriggerStay(Collider other)
        {
            //This is to see if the player is within a certain distance of the AI.
            if (other.gameObject.tag == "Player" )
            {
            //Only do this on tag "Player"
			Vector3 direction  = other.transform.position - this.transform.position;
			float angle = Vector3.Angle(direction, transform.forward);//Draw the angle in front of the AI
			
			if(angle < fov * 0.5f)//This is the angle that the AI can see
			{
				RaycastHit hit ;
				
				if(Physics.Raycast(transform.position + transform.up, direction.normalized, out hit, 0))//Check if the AI can see the player
				{
					if(hit.collider.tag == "Player")
                    {
                            InAgroRange();
                            this.agro = true;
                            this.InAgroRange();
                        }
					else if(smart)
					{
                            smarts.GoToNextPoint();

					}
                    else
                    {

                    }
					
				}
			}

				
				//AudioSource audios = other.GetComponent<AudioSource>();
			//	float audiorange = audios.maxDistance;
			//	if(audios.isPlaying == true)
				//{
				////	Player = FindClosestPlayer();
				//	PlayerT = Player.transform;
				//	DoWaypoints = false;
				//	GetComponent<NavMeshAgent>().destination = PlayerT.position;
				//}
				//else
				//{
				//	DoWaypoints = true; 
				//}



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
