using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Movement : MonoBehaviour
{
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
        agent.baseOffset = 1;
    }

    public void SetGoal(Transform goal, float stopDist = 1)
    {
        this.goal = goal;

        if (goal == null)
        {
            agent.isStopped = true;
            return;
        }

        agent.isStopped = false;
        agent.speed = speed;
        agent.stoppingDistance = stopDist;
        this.stopDist = stopDist;

        agent.SetDestination(goal.position);
    }

    void Update()
    {
        try
        {
            if (goal != null)
                agent.SetDestination(goal.position);
        }
        catch { }
    }

    public bool IsReached()
    {
        return goal != null && agent.remainingDistance <= agent.stoppingDistance;
    }
}
