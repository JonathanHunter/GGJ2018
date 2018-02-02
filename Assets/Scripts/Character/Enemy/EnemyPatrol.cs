namespace GGJ2018.Character.Enemy
{
    using UnityEngine;
    using UnityEngine.AI; 
    using GGJ2018.Managers;
    using GGJ2018.Character.Enemy.Smarts;

    public class EnemyPatrol : Enemy 
    {
        public EnemyAI smarts;
        public Transform[] points; // locations to patr
       
        new public void GotoNextPoint()
        {
            if (points[0] != null)
            {
                // Returns if no points have been set up
                if (points.Length == 0)
                    return;

                // Set the agent to go to the currently selected destination.
                agent.destination = points[destPoint].position;

                // Choose the next point in the array as the destination,
                // cycling to the start if necessary.
                destPoint = (destPoint + 1) % points.Length;
            }

        }

        new void TargetPlayer()
        {
            agent.transform.LookAt(PlayerManager.Instance.player);
        }

        public Vector3 RandomNavmeshLocation(float radius)
        {
            Vector3 randomDirection = Random.insideUnitSphere * radius;
            randomDirection += agent.transform.position;
            NavMeshHit hit;
            Vector3 finalPosition = Vector3.zero;
            if (NavMesh.SamplePosition(randomDirection, out hit, radius, 1))
            {
                finalPosition = hit.position;
            }
            return finalPosition;
        }

        
        
        protected override void LocalInit()
        {
            maxHealth = 3;
            smarts = new EnemyAI(this);
            agent = GetComponent<NavMeshAgent>();
            // points[0].position = agent.transform.position;
            // Disabling auto-braking allows for continuous movement
            // between points (ie, the agent doesn't slow down as it
            // approaches a destination point).
            agent.autoBraking = false;
            agent.speed = 0.5f;
            this.points[0] = agent.transform;
            GotoNextPoint();
        }

        protected override void LocalUpdate()
        {
            // Choose the next destination point when the agent gets
            // close to the current one.
            if (this.agro)
            {
                smarts.chase();
            }

            if (!agent.pathPending && agent.remainingDistance < 0.5f && !this.agro)
            {
                smarts.GotoNextPoint(); // search for player
            }

        }

        protected override void TookDamage()
        {
            sfx.PlayEnemyGetHitSFX();
            float rate = this.Health / maxHealth;
            agent.speed = agent.speed - rate;
        }
        
        protected override void InAgroRange()
        {
          TargetPlayer();
        }
    }
}

