using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worker : Person
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

}
