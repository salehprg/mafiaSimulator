using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Investor : Person
{
    public float investAmount;
    public GameObject coinAnim;

    public override void OnStart()
    {
        waitingroom = null;
        personStatus = PersonStatus.Working;
        SetTarget(this);
    }

    public override void ReachTarget(ITargetable _target)
    {

    }

    public override void DoingJob(ITargetable target)
    {
        if (waitTime - Time.time < 0)
        {
            wallet.DepositBalance(investAmount);
            waitTime = Time.time + idleTime;
            
            FinishJobPlayAnim(target);
        }
    }

    private void Update()
    {
        base.Update();
        if (health > 0)
        {
            if (personStatus == PersonStatus.Dead || personStatus == PersonStatus.Idle)
            {
                SetTarget(this);
                personStatus = PersonStatus.Working;
            }
        }

    }

    public override void FinishJobPlayAnim(ITargetable target)
    {
        var tmp = Instantiate(coinAnim, (target as MonoBehaviour).transform.position, new Quaternion());
        tmp.transform.SetParent((target as MonoBehaviour).transform);
        Destroy(tmp, 1f);
    }
}
