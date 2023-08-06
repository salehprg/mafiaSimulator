using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Miner : Person
{
    public Building building;


    public override void ReachTarget(ITargetable _target)
    {
        building = (Building)_target;

        waitTime = Time.time + building.waitTime;
    }

    public override void DoingJob()
    {
        if (waitTime - Time.time < 0)
        {
            float amount = building.GetMoney();
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
