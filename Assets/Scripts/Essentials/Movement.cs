using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public Transform goal;
    public float speed = 1;
    public bool reachGoal;

    float stopDist;

    public void SetGoal(Transform goal, float stopDist = 1)
    {
        this.goal = goal;
        this.stopDist = stopDist;
    }

    void Update()
    {
        if (goal != null)
        {
            if (Vector3.Distance(transform.position, goal.position) > stopDist)
            {
                reachGoal = false;
                var newPos = Vector3.MoveTowards(transform.position, goal.position, speed * Time.deltaTime);

                var dest = new Vector3(newPos.x, transform.position.y, newPos.z);
                transform.position = dest;
            }
            else
            {
                reachGoal = true;
            }
        }
    }
}
