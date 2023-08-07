using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Healer : Person
{
    public float healTime;
    public float payment;

    public override void ReachTarget(ITargetable _target)
    {
        waitTime = Time.time + healTime;
    }

    public override void DoingJob(ITargetable target)
    {
        if (waitTime - Time.time < 0)
        {
            (target as Person).Heal();
            float amount =  (target as Person).GetWallet().WithdrawBalance(payment);
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
