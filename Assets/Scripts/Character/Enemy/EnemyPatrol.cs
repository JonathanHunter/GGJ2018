namespace GGJ2018.Character.Enemy
{
    using UnityEngine;
    using UnityEngine.AI; 
    using GGJ2018.Managers;
    using GGJ2018.Character.Enemy.Smarts;
    

    public class EnemyPatrol : Enemy 
    {
        public EnemyAI smarts; //for future use with position variation
        public Transform[] points; // locations to patr
        public float reset;  // reset for cooldown
       
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
        /// Chase player
       public override void Chase()
        {
            Vector3 dist = PlayerManager.Instance.player.position -  agent.transform.position;  //Chase the player to the agroRange distance which is the minimum distance needed to shoot
            if (dist.magnitude >  agroRange)
            {
                 agent.destination = PlayerManager.Instance.player.position;
            }
            else if (dist.magnitude <=  agroRange)
            {
                 agent.transform.LookAt(PlayerManager.Instance.player);
                 agent.destination =  agent.transform.position;
            }

            //shoot after given time if you have LOS then reset cooldown 
            if (( cooldown -= Time.deltaTime) <= 0 && los)
            {
                 enemyAnimator.SetBool("fireGun", true);
                 sfx.PlayEnemyGunfireSFX();
                 Shoot();
                 cooldown = reset;

                enemyAnimator.SetBool("fireGun", true);
                sfx.PlayEnemyGunfireSFX();
                cooldown = 5f;
            }
             shooting = false;
            // chase for given time then roam remove agro if loss of LOS
        }  
        
        protected override void LocalInit()
        {
            reset = cooldown;
            maxHealth = 3;
            smarts = new EnemyAI(this);
            agent = GetComponent<NavMeshAgent>();
        
            // Disabling auto-braking allows for continuous movement
            // between points (ie, the agent doesn't slow down as it
            // approaches a destination point).
            agent.autoBraking = false;
            GotoNextPoint();
        }

        protected override void LocalUpdate()
        {
            // Choose the next destination point when the agent gets
            // close to the current one.
            if (this.agro && level > 1)
            {
                TargetPlayer();
                Chase();
            }
            else if (this.agro && level ==1)
            {
                TargetPlayer();
                if ((cooldown -= Time.deltaTime) <= 0 && los)
                {
                    enemyAnimator.SetBool("fireGun", true);
                    sfx.PlayEnemyGunfireSFX();
                    Shoot();
                    cooldown = reset;
                }
            }

            if (!agent.pathPending && agent.remainingDistance < 0.5f && !this.agro)
            {
                GotoNextPoint(); // search for player
            }

        }

        protected override void TookDamage()
        {
            sfx.PlayEnemyGetHitSFX();
            agent.speed = agent.speed+=0.1f ; // agent speed increase as health decrease
        }
        
        protected override void InAgroRange()
        {
          TargetPlayer();
        }
    }
}

