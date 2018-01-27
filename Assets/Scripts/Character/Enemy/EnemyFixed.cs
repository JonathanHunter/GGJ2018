// Patrol.cs
using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using GGJ2018.Character.Enemy;
using GGJ2018.Character.Player;
using GGJ2018.Managers;

public class EnemyFixed : Enemy
{
    public PlayerManager playerManager;
    public Transform[] points; // locations to patrol
    public Transform startPos;
    private int destPoint = 0;
    private float agroRange = 15f;
    private float minAttackRange = 3f;
    private float fov = 90f;
    private NavMeshAgent agent;



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
        Vector3 dist = playerManager.player.position - agent.transform.position;
        if (dist.magnitude > 3f)
        {
            agent.speed = 1.0f; // increase speed to chase 
            print("Chase");
            agent.destination = playerManager.player.position;
        }
        else if (dist.magnitude <= 3f)
        {
            print("Fixed Fire");
            agent.destination = agent.transform.position;
        }
        this.Shoot();

    }

    void TargetPlayer()
    {
        Vector3 direction = playerManager.player.position - this.transform.position;
        float angle = Vector3.Angle(direction, transform.forward);//Draw the angle in front of the AI
        this.transform.Rotate(direction, angle / 2);
    }
    protected override void LocalInit()
    {
        maxHealth = 100;

        agent = GetComponent<NavMeshAgent>();
        // points[0].position = agent.transform.position;
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
        /*
        if (!agent.pathPending && agent.remainingDistance < 0.5f && !this.agro)
        {
            GotoNextPoint(); // search for player
        }
        */
    }

    protected override void TookDamage()
    {
        sfx.PlayEnemyGetHitSFX();
        //Health = this.Health - 20;
        float rate = this.Health / maxHealth;
        agent.speed = agent.speed - rate;

    }


    protected override void InAgroRange()
    {
        print("agro");
        TargetPlayer();
    }
}

