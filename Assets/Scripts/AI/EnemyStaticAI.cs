using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{

    public Transform destination;
    public Transform startPos;
    private float agroRange = 15f;
    private float minAttackRange = 3f;
    private float fov = 90f;
    private NavMeshAgent agent;
    bool agro;


    void Start()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();

    }

    void attack()
    {
        //face player
        //agent.SetDestination = player.
        if (agent.remainingDistance < minAttackRange)
        {
            // stop moving to attack
        }
    }

    void GotoNextPoint()
    {
        // Set the agent to go to the start position.
        agent.destination = startPos.transform.position;
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
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            agent.destination = startPos.transform.position;
        }
        else
        {
            attack();
        }
        
    }

}