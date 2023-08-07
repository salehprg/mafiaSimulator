using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Thief : Person
{
    public float stoletime;
    public float moneyStole;
    
    public override void ReachTarget(ITargetable _target)
    {
        waitTime = Time.time + stoletime;
    }

    public override void DoingJob(ITargetable target)
    {
        if (waitTime - Time.time < 0)
        {
            var money = 0.0f;

            if (target.GetType().BaseType == typeof(Person))
                money = ((Person)target).GetWallet().WithdrawBalance(moneyStole);

            if (target.GetType().BaseType == typeof(Building))
                money = ((Building)target).GetMoney();

            wallet.DepositBalance(money);

            FinishJob();
        }
    }

    public override List<ITargetable> GetMyTargets()
    {
        var _targets = targets.Where(x => x.GetType().BaseType == typeof(Person) 
                                        && ((Person)x).personStatus != PersonStatus.Prison 
                                        && ((Person)x).personStatus != PersonStatus.Catched);


        return _targets.ToList();
    }
}
