using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Investor : Person
{   
    public float investAmount;

    public override void Start(){
        waitingroom = null;
        personStatus = PersonStatus.DoNothing;
    }

    public override void ReachTarget(ITargetable _target)
    {
        
    }

    public override void Update()
    {
        base.Update();

        if(waitTime - Time.time < 0){
            wallet.DepositBalance(investAmount);
            waitTime = Time.time + idleTime;
        }
    }

    public override void DoingJob(){}

}
