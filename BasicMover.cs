using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent (typeof(NavMeshAgent))]
public class BasicMover : MonoBehaviour
{
    NavMeshAgent agent = null;

    [SerializeField] Transform[] waypoints = null;

    private int currentWP = -1;

    const float CLOSE_DIST = 2.0f;
    const float GET_NEXT_CD = .25f;

    private float cooldown = 0; //So the agent does call GetNextWaypoint a ton of times (via Update)

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if (waypoints.Length == 0)
            throw new System.Exception($"{name} needs waypoints!");

        currentWP = 0;
    }

    void Update()
    {
        if (CloseToCurrentWaypoint) //It will be close for a few frames
            MoveToNextWaypoint(); //So this will get called more than once

        if (cooldown > 0)
            cooldown -= Time.deltaTime;
    }

    private void MoveToNextWaypoint()
    {
        if (cooldown > 0) //That's why I put in a cooldown
            return;
            
        Vector3 pos = GetNextWaypoint();

        agent.SetDestination(pos);
        agent.isStopped = false;

        cooldown = GET_NEXT_CD;
    }

    private Vector3 GetNextWaypoint() 
    {
        if (currentWP == waypoints.Length - 1)
            currentWP = 0;
        else
            currentWP++;

        return waypoints[currentWP].position;
    }

    private bool CloseToCurrentWaypoint => agent.remainingDistance <= CLOSE_DIST;
}
