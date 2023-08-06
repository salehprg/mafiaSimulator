using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Thief : Person
{
    public float stoletime;
    public float moneyStole;
    public ITargetable myTarget;


    public override void ReachTarget(ITargetable _target)
    {
        myTarget = _target;

        waitTime = Time.time + stoletime;
    }

    public override void DoingJob()
    {
        if (waitTime - Time.time < 0)
        {
            var money = 0.0f;

            if (myTarget.GetType().BaseType == typeof(Person))
                money = ((Person)myTarget).wallet.WithdrawBalance(moneyStole);

            if (myTarget.GetType().BaseType == typeof(Building))
                money = ((Building)myTarget).GetMoney();

            wallet.DepositBalance(money);

            FinishJob();
        }
    }

    public override List<ITargetable> GetMyTargets()
    {
        var _targets = targets.Where(x => x.GetType().BaseType == typeof(Person) 
                                        && (((Person)x).personStatus != PersonStatus.Prison || ((Person)x).personStatus != PersonStatus.Catched));


        return _targets.ToList();
    }
}
