using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Healer : Person
{
    public float healTime;
    public float payment;
    public GameObject healAnim;

    public override void ReachTarget(ITargetable _target)
    {
        waitTime = Time.time + healTime;
    }

    public override void DoingJob(ITargetable target)
    {
        if (waitTime - Time.time < 0)
        {
            (target as Person).Heal();
            float amount = (target as Person).GetWallet().WithdrawBalance(payment);
            wallet.DepositBalance(amount);

            FinishJob();
        }
    }

    public override List<ITargetable> GetMyTargets()
    {
        return targets.Where(x => ((Person)x).personStatus == PersonStatus.Dead).ToList();
    }

    public override void FinishJobPlayAnim(ITargetable target)
    {
        var tmp = Instantiate(healAnim, (target as MonoBehaviour).transform.position, new Quaternion());
        tmp.transform.SetParent((target as MonoBehaviour).transform);
        Destroy(tmp, 1f);
    }
}
