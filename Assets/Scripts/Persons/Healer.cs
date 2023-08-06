using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Healer : Person
{
    public float hillTime;
    public float payment;
    public Person myTarget;


    public override void ReachTarget(ITargetable _target)
    {
        myTarget = (Person)_target;
        waitTime = Time.time + hillTime;
    }

    public override void DoingJob()
    {
        if (waitTime - Time.time < 0)
        {
            myTarget.health = 100;
            float amount = myTarget.wallet.WithdrawBalance(payment);
            wallet.DepositBalance(amount);

            FinishJob();
        }
    }

    public override List<ITargetable> GetMyTargets()
    {
        var _targets = targets;
        _targets = _targets.Where(x => ((Person)x).personStatus == PersonStatus.Dead).ToList();

        return _targets;
    }

}
