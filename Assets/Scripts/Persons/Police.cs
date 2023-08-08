using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Police : Person
{
    public float radius;
    public float jailTime;
    public Transform prison;

    bool goingToJail = false;
    float _speed;

    public bool Bribe(ITargetable target, float amount)
    {
        if (amount > 0)
        {
            movement.speed = _speed;

            wallet.DepositBalance(amount);
            GoToWaitingRoom();

            (target as Person).GoToWaitingRoom();
            (target as Person).FinishJob();
            FinishJob();

            return true;
        }

        return false;
    }
    
    public override void OnAwake()
    {
        prison = GameManagerr.instance.prisons[Random.Range(0, GameManagerr.instance.prisons.Length)].transform;
    }

    public override void OnStart(){
        waitingroom = GameManagerr.instance.policeStations[Random.Range(0, GameManagerr.instance.policeStations.Length)];
    }


    public override void ReachTarget(ITargetable _target)
    {
        goingToJail = true;

        var myTarget = _target as Person;

        _speed = movement.speed;
        movement.speed = myTarget.GetMovement().speed;
        SetNewPosition(myTarget.transform);

        myTarget.CatchByPolice();
        myTarget.SetNewPosition(prison.transform);
    }

    public override void DoingJob(ITargetable target)
    {
        if(goingToJail && Vector3.Distance(transform.position , prison.position) < 5){
            goingToJail = false;
        }

        if (movement.IsReached() && !goingToJail)
        {
            (target as Person).KeepInJail(jailTime);
            (target as Person).SetNewPosition(prison);
            movement.speed = _speed;

            FinishJob();
        }
    }


    public override List<ITargetable> GetMyTargets()
    {
        return targets.Where(x => ((Person)x).personStatus == PersonStatus.Idle 
                                    || ((Person)x).personStatus == PersonStatus.Working 
                                    || ((Person)x).personStatus == PersonStatus.GoingToWork).ToList();
    }

}
