// Patrol.cs
using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using GGJ2018.Character.Enemy;
using GGJ2018.Character.Player;

public class EnemyAi : Enemy
{
    public Player player;
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
        Vector3 position = new Vector3(Random.Range(-2.0f, 2.0f), 0, Random.Range(-2.0f, 2.0f));
        agent.destination = position;
 
    }

    void attack()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            GotoNextPoint();
        }
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
        if (agent.remainingDistance > 1f)
        {
            GotoNextPoint();
           agent.destination = player.transform.position;
        }
        else
        {
            agent.isStopped = true; 
        }
        attack();
       
    }
}

