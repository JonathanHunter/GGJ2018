using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{

    public Transform[] points;
    private int destPoint = 0;
    private int aRange = 0;
    private NavMeshAgent agent;
    private float distance;
    bool playerInSight;
    bool doWayPoints;
    float fov;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        // Disabling auto-braking allows for continuous movement
        // between points (ie, the agent doesn't slow down as it
        // approaches a destination point).
        agent.autoBraking = false;

        GotoNextPoint();
    }


    void GotoNextPoint()
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

   void OnTriggerStay(Collider other)
    {//This is to see if the player is within a certain distance of the AI.
        if (other.gameObject.tag == "Player")
        {//Only do this on ppl tagged "Player"

            Vector3 direction = agent.transform.position - this.transform.position;

            float angle = Vector3.Angle(direction, transform.forward);//Draw the angle in front of the AI

            if (angle < 90 * 0.5f)//This is the angle that the AI can see
            {
                RaycastHit hit;

                if (Physics.Raycast(transform.position + transform.up, direction.normalized, out hit, -1))//Check if the AI can see the player
                {
                    if (hit.collider.tag == "Player")
                    {
                        playerInSight = true; //AI Character sees the player
                        doWayPoints = false; //don't walk waypoints anymore
                    }
                    else
                    {
                        doWayPoints = true;
                        playerInSight = false;
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
            GotoNextPoint();

    }

}