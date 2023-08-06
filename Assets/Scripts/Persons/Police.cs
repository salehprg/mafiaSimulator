using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Police : Person
{
    public float radius;
    public float jailTime;
    public Person myTarget;
    public Transform prison;

    float _speed;

    public bool Bribe(float amount)
    {
        if (amount > 0)
        {
            movement.speed = _speed;
            
            wallet.DepositBalance(amount);
            GoToWaitingRoom();

            myTarget.GoToWaitingRoom();
            myTarget.FinishJob();
            FinishJob();

            return true;
        }

        return false;
    }

    public override void Start()
    {
        base.Start();
        waitingroom = center.gameObject;
        _speed = movement.speed;
    }

    public override void ReachTarget(ITargetable _target)
    {
        myTarget = (Person)_target;
    }

    public override void DoingJob()
    {
        SetNewPosition(prison.transform);
        movement.speed = myTarget.movement.speed;

        myTarget.CatchByPolice();
        myTarget.SetNewPosition(prison);

        if (Vector3.Distance(transform.position, prison.position) < 1)
        {
            myTarget.KeepInJail(jailTime);
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
