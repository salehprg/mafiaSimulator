using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Police : Person
{
    public float radius;
    public float jailTime;
    public Transform prison;

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
        prison = GameManager.instance.prisons[Random.Range(0, GameManager.instance.prisons.Length)].transform;
        waitingroom = GameManager.instance.policeStations[Random.Range(0, GameManager.instance.policeStations.Length)];
    }

    public override void ReachTarget(ITargetable _target)
    {
        var myTarget = _target as Person;

        _speed = movement.speed;
        movement.speed = myTarget.movement.speed;
        SetNewPosition(prison.transform);

        myTarget.CatchByPolice();
        myTarget.SetNewPosition(this.transform);
    }

    public override void DoingJob(ITargetable target)
    {
        if (movement.IsReached())
        {
            (target as Person).KeepInJail(jailTime);
            (target as Person).SetNewPosition(prison);
            movement.speed = _speed;

            FinishJob();
        }
    }


    public override List<ITargetable> GetMyTargets()
    {
        var _targets = new List<ITargetable>();
        _targets.AddRange(targets);

        foreach (var targ in targets)
        {
            var prsn = (Person)targ;

            if (prsn.personStatus == PersonStatus.Dead || prsn.personStatus == PersonStatus.Prison || prsn.personStatus == PersonStatus.Catched
                || Vector3.Distance(((MonoBehaviour)targ).transform.position, transform.position) > radius)
            {
                _targets.Remove(targ);
            }
        }

        return _targets;
    }

}
