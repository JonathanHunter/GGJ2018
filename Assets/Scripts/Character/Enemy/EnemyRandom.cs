namespace GGJ2018.Character.Enemy
{
    using UnityEngine;
    using UnityEngine.AI;
    using Managers;

    public class EnemyRandom : Enemy
    {
        public Transform[] points; // locations to patrol
        public Transform startPos;
        private int destPoint = 0;
        private float agroRange = 5f;
        private float minAttackRange = 3f;
        private float fov = 90f;
        public float cooldown = 5;


        void GotoNextPoint()
        {
            // Set the agent to go to the currently selected destination

            // Vector3 position = new Vector3(Random.Range(-3.0f,3.0f), 0, Random.Range(-3.0f, 3.0f)) + this.transform.position  ;
            agent.destination = RandomNavmeshLocation(Random.Range(-10.0f, 10.0f));
            //agent.destination = playerManager.player.position;
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

        void attack()
        {
            enemyAnimator.SetBool("isMoving", true);
            Vector3 dist = PlayerManager.Instance.player.position - agent.transform.position;
            if (dist.magnitude > agroRange)
            {
                agent.speed = 1.0f; // increase speed to chase 
                agent.destination = PlayerManager.Instance.player.position;
            }
            else if (dist.magnitude <= agroRange)
            {
                enemyAnimator.SetBool("isMoving", false);
                agent.transform.LookAt(PlayerManager.Instance.player);
                agent.destination = agent.transform.position;
            }

            if ((cooldown -= Time.deltaTime) <= 0)
            {
                enemyAnimator.SetBool("fireGun", true);
                sfx.PlayEnemyGunfireSFX();
                this.Shoot();
                cooldown = 5f;
            }
            shooting = false;
        }

        void TargetPlayer()
        {
            agent.transform.LookAt(PlayerManager.Instance.player);
        }

        protected override void LocalInit()
        {

            agent = GetComponent<NavMeshAgent>();
            // Disabling auto-braking allows for continuous movement
            // between points (ie, the agent doesn't slow down as it
            // approaches a destination point).
            agent.autoBraking = false;
            agent.speed = 0.5f;
            GotoNextPoint();

        }

        protected override void LocalUpdate()
        {
            // Choose the next destination point when the agent gets
            // close to the current one.
            if (this.agro)
            {
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
            float rate = this.Health / maxHealth;
            agent.speed = agent.speed - rate;

        }


        protected override void InAgroRange()
        {
             TargetPlayer();
        }
    }
}

