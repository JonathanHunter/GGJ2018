namespace GGJ2018.Character.Enemy.Smarts
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.AI;
    using GGJ2018.Managers;

    public class EnemyAI : MonoBehaviour
    {
        private Enemy enemy;
        public Transform[] points; // locations to patr 
        private int loc = 0; // iterate through waypoints  
        
        public EnemyAI(Enemy enemy)
        {
            this.enemy = enemy;
        }


        /*
         Chase Player If agro
         */
        public void chase()
        {
            enemy.enemyAnimator.SetBool("isMoving", true);
            Vector3 dist = PlayerManager.Instance.player.position - enemy.agent.transform.position;
            if (dist.magnitude > enemy.agroRange)
            {
                enemy.agent.speed = 1.0f; // increase speed to chase 
                enemy.agent.destination = PlayerManager.Instance.player.position;
            }
            else if (dist.magnitude <= enemy.agroRange)
            {
                enemy.enemyAnimator.SetBool("isMoving", false);
                enemy.agent.transform.LookAt(PlayerManager.Instance.player);
                enemy.agent.destination = enemy.agent.transform.position;
            }

            if ((enemy.cooldown -= Time.deltaTime) <= 0)
            {
                enemy.enemyAnimator.SetBool("fireGun", true);
                enemy.sfx.PlayEnemyGunfireSFX();
                enemy.Shoot();
                enemy.cooldown = 5f;
            }
            enemy.shooting = false;
            // chase for given time then roam

        }


        /*
         Find random location within navmesh
         */
        public Vector3 RandomNavmeshLocation(float radius)
        {
            Vector3 randomDirection = Random.insideUnitSphere * radius;
            randomDirection += enemy.agent.transform.position;
            NavMeshHit hit;
            Vector3 finalPosition = Vector3.zero;
            if (NavMesh.SamplePosition(randomDirection, out hit, radius, 1))
            {
                finalPosition = hit.position;
            }
            return finalPosition;
        }

        new public void GotoNextPoint()
        {
            enemy.agent.destination = transform.position;
            // Returns if no points have been set up
            if (points[loc] = null)
                return;
            // Set the agent to go to the currently selected destination.

            //Set the enemy to each way point and return if none was entered. 
            if (points[loc] != null && enemy.agent.remainingDistance < 0.1f)
            {
                enemy.agent.destination = points[loc].position;
                loc++;
            }
            else
            {
                enemy.agent.destination = points[loc].position;
                loc = 0;
            }
            // Choose the next point in the array as the destination,
            // cycling to the start if necessary.
            // destPoint = (destPoint + 1) % points.Length;
        }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}