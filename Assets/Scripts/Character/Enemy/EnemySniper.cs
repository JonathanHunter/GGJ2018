namespace GGJ2018.Character.Enemy
{
    using UnityEngine;
    using UnityEngine.AI;
    using GGJ2018.Managers;
    //using GGJ2018.Character.Enemy.Smarts;

    /*
     * CANT OPEN UNITY AT WORK TO CHANGE FILE NAME BUT THIS IS SNIPER SCRIPT NOW
     */
    public class EnemySniper : Enemy
    {
        public Transform[] points; // locations to patr
        public float reset;  // reset for cooldown

        void OnTriggerStay(Collider other)
        {
            Vector3 direction = other.transform.position - this.gunPos.transform.position;
            float angle = Vector3.Angle(direction, transform.forward);//Draw the angle in front of the AI

            if (angle < fov * 0.5f)//This is the angle that the AI can see
            {
                RaycastHit hit;

                if (Physics.Raycast(this.gunPos.transform.position, other.transform.position - this.gunPos.transform.position, out hit) && hit.transform.tag == "Player")
                {
                    if (hit.collider.tag == "Player")
                    {
                        InAgroRange();
                        this.agro = true;
                        this.InAgroRange();
                        los = true;
                        //agro animation
                    }
                    else
                    {
                        los = false;
                        if (((interest) -= Time.deltaTime) <= 0 && !los)
                        {
                            agro = false;
                            GotoNextPoint();
                            //lose interest in the player
                        }
                    }
                }
            }
        }

        public override void GotoNextPoint()
        {
            // Returns if no points have been set up
            if (points.Length == 0 || this.agro)
                return;

            // Set the agent to go to the currently selected destination.
            agent.destination = points[destPoint].position;

            // Choose the next point in the array as the destination,
            // cycling to the start if necessary.
            destPoint = (destPoint + 1) % points.Length;
        }
        /// Chase for given time then roam removes agro if loss of LOS
        public override void Chase()
        {
            //Chase the player to the agroRange distance which is the minimum distance needed to shoot
            Vector3 dist = PlayerManager.Instance.player.position - agent.transform.position;  
            if (dist.magnitude > agroRange)
            {
                agent.destination = PlayerManager.Instance.player.position;
            }
            else if (dist.magnitude <= agroRange)
            {
                agent.transform.LookAt(PlayerManager.Instance.player);
                agent.destination = agent.transform.position;
            }
            attack(); 
        }

        public override void attack()
        {
            //shoot after given time if you have LOS then reset cooldown 
            if ((cooldown -= Time.deltaTime) <= 0 && los && agent.enabled)
            {
                shooting = true;
              //enemyAnimator.SetBool("fireGun", true); // sniper animation
              //sfx.PlayEnemyGunfireSFX();
                Shoot();
                cooldown = reset;
            }
            shooting = false;
        }

        protected override void LocalInit()
        {
            reset = cooldown;
            maxHealth = 3;
            agent = GetComponent<NavMeshAgent>();

            // Disabling auto-braking allows for continuous movement
            // between points (ie, the agent doesn't slow down as it
            // approaches a destination point).
            agent.autoBraking = false;
            enemyAnimator.SetBool("isMoving", false);
            GotoNextPoint();
        }

        protected override void LocalUpdate()
        {
            if (!agent.enabled)
                agent.enabled = true;
            //if the agent is moving start the animation or else don't
            if (agent.hasPath)
            {
                enemyAnimator.SetBool("isMoving", true);
                enemyAnimator.speed = agent.speed * animRatio; // animation speed reflects agent speed
            }
            else if (agent.remainingDistance >= 0.75f && agent.remainingDistance <= 1)
            {
                enemyAnimator.SetBool("isMoving", true);
                enemyAnimator.speed = 0.75f; // animation speed reflects agent speed
            }
            else if (!agent.hasPath)
            {
                enemyAnimator.SetBool("isMoving", false);
                enemyAnimator.speed = 1f * animRatio; // animation speed reflects agent speed
            }

            // Choose the next destination point when the agent gets
            // close to the current one.
            if (this.agro && level > 1)
            {
                TargetPlayer();
                attack();
                //Chase();
            }
            else if (this.agro && level == 1)
            {
                TargetPlayer();
                attack();
            }
             
            if (!agent.pathPending && agent.remainingDistance < 0.5f && !this.agro)
            {
                GotoNextPoint(); // search for player
            }

        }

        protected override void TookDamage()
        {
            sfx.PlayEnemyGetHitSFX();
            agent.speed = agent.speed += 0.1f; // agent speed increase as health decrease
        }

        protected override void InAgroRange()
        {
            TargetPlayer();
        }
    }
}

