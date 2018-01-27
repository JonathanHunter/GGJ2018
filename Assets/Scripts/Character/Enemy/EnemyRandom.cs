// Patrol.cs
using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using GGJ2018.Character.Enemy;
using GGJ2018.Character.Player;
using GGJ2018.Managers;

public class EnemyRandom : Enemy
{
    public PlayerManager playerManager;
    public Transform[] points; // locations to patrol
    private int destPoint = 0;
    private float agroRange = 15f;
    private float minAttackRange = 3f;
    private float fov = 90f;
    private NavMeshAgent agent;

    bool agro = false;

    void GotoNextPoint()
    {
        // Set the agent to go to the currently selected destination

        // Vector3 position = new Vector3(Random.Range(-3.0f,3.0f), 0, Random.Range(-3.0f, 3.0f)) + this.transform.position  ;


       agent.destination = RandomNavmeshLocation(Random.Range(-10.0f, 10.0f));
        //agent.destination = playerManager.player.position;
        print("next location");
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


    
      
    }

    void TargetPlayer()
    {
        Vector3 direction = playerManager.player.position - this.transform.position;
        float angle = Vector3.Angle(direction, transform.forward);//Draw the angle in front of the AI
        this.transform.Rotate(direction, angle);
    }
    protected override void LocalInit()
    {
        Health = 100;
       
        agent = GetComponent<NavMeshAgent>();
        // points[0].position = agent.transform.position;
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
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            GotoNextPoint();
        }
      
    }

    protected override void TookDamage()
    {
        sfx.PlayEnemyGetHitSFX();
        //Health = this.Health - 20;
        float rate = this.Health / maxHealth  ;
        agent.speed = 1.1f - rate;

    }

    protected override void InAgroRange()
    {
        agro = true;
        print("agro");
        if (agent.remainingDistance > 2f )
        {
            print("Chase");
            agent.destination = playerManager.player.transform.position;
        }
        else
        {
            print("Fixed Fire");
            agent.isStopped = true;
        }
        attack();
       
    }
}

