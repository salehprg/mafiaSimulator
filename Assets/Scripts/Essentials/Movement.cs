using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

[RequireComponent(typeof(NavMeshAgent))]
public class Movement : MonoBehaviour
{
    public UnityEvent<float> eventSetSpeed;
    
    protected NavMeshAgent agent;
    public Transform goal;
    public float speed = 1;
    public bool isReached;

    float stopDist;

    private void Start()
    {
        isReached = false;
        agent = GetComponent<NavMeshAgent>();
        agent.enabled = true;
        agent.baseOffset = 0;
    }

    public void Pause(){
        agent.isStopped = true;
    }
    public void Resume(){
        agent.isStopped = false;
    }

    public void SetGoal(Transform goal, float stopDist = 1)
    {
        this.goal = goal;
        try
        {
            if (goal == null)
            {
                Pause();
                return;
            }

            Resume();
            agent.speed = speed;
            agent.stoppingDistance = stopDist;
            this.stopDist = stopDist;

            agent.SetDestination(goal.position);
        }
        catch (Exception) { }
    }

    void Update()
    {
        try
        {
            eventSetSpeed?.Invoke(agent.velocity.magnitude / agent.speed);

            if (goal != null)
                agent.SetDestination(goal.position);
        }
        catch { }
    }

    public bool IsReached()
    {
        return goal != null && Vector3.Distance(transform.position , agent.destination) <= agent.stoppingDistance;
    }
}
