﻿// Patrol.cs
using UnityEngine;
using UnityEngine.AI;
using System.Collections;


public class EnemyPatrol : MonoBehaviour
{
    
    public Transform[] points; // locations to patrol
    private int destPoint = 0;
    private float agroRange = 15f;
    private float minAttackRange = 3f;
    private float fov = 90f;
    private NavMeshAgent agent;
 
    bool agro = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
       // points[0].position = agent.transform.position;
        // Disabling auto-braking allows for continuous movement
        // between points (ie, the agent doesn't slow down as it
        // approaches a destination point).
        agent.autoBraking = false;

        GotoNextPoint();
    }


    void GotoNextPoint()
    {
        //agent = GetComponent<NavMeshAgent>();
        // Rds(points.Length == 0)
        //return;

        // Set the agent to go to the currently selected destination.
        // agent.destination = points[destPoint].position;
        Vector3 position = new Vector3(Random.Range(-5.0f, 5.0f), 0, Random.Range(-5.0f, 5.0f));
        agent.destination = position;
        // Choose the next point in the array as the destination,
        // cycling to the start if necessary.
        //destPoint = (destPoint + 1) % points.Length;
    }
    
    void attack()
    {
        agro = true;
        //face player
        //agent.SetDestination = player.
        if (agent.remainingDistance < minAttackRange )
        {
          // stop to attack
        }
        else if(agent.remainingDistance > agroRange)
        {
            agro = false;
            // goes to next way point 
        }
        //if range attack player
    }
    void OnTriggerEnter(Collider other)
    {//This is to see if the player is within a certain distance of the AI.
        if (other.gameObject.tag == "Player")
        {//Only do this on ppl tagged "Player"

            Vector3 direction = other.transform.position - this.transform.position;

            float angle = Vector3.Angle(direction, transform.forward);//Draw the angle in front of the AI

            if (angle < fov * 0.5f)//This is the angle that the AI can see
            {
                RaycastHit hit;

                if (Physics.Raycast(transform.position + transform.up, direction.normalized, out hit, -1))//Check if the AI can see the player
                {
                    if (hit.collider.tag == "Player")
                    {
                        agro = true; //AI Character sees the player
                        attack();
                        //DoWaypoints = false; //don't walk waypoints anymore
                    }
                    else
                    {
                        //DoWaypoints = true;
                        agro = false;
                        GotoNextPoint(); // Idle 
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

    void Update()
    {      
        // Choose the next destination point when the agent gets
        // close to the current one.
        if (!agent.pathPending && agent.remainingDistance < 0.5f && !agro)
        {
            GotoNextPoint();
        }
   

    }
}

