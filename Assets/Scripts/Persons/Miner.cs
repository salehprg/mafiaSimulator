using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Miner : Person
{
    public override void ReachTarget(ITargetable _target)
    {
        waitTime = Time.time + (_target as Building).waitTime;
    }

    public override void DoingJob(ITargetable _target)
    {
        if (waitTime - Time.time < 0)
        {
            float amount = (_target as Building).GetMoney();
            wallet.DepositBalance(amount);

            FinishJob();
        }
    }

    public override List<ITargetable> GetMyTargets()
    {
        var _targets = targets;
        _targets = _targets.Where(x => ((Building)x).store_money > 0).ToList();

        return _targets;
    }
}
