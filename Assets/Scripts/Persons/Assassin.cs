using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Assassin : Person
{
    public float killTime;
    public Person myTarget;


    public override void ReachTarget(ITargetable _target)
    {
        myTarget = (Person)_target;
        waitTime = Time.time + killTime;
    }

    public override List<ITargetable> GetMyTargets()
    {
        var _targets = targets;
        _targets = _targets.Where(x => x != ((ITargetable)this)
        && (((Person)x).personStatus != PersonStatus.Prison || ((Person)x).personStatus != PersonStatus.Catched)
         && ((Person)x).health > 0).ToList();

        return _targets;
    }

    public override void DoingJob()
    {
        if (waitTime - Time.time < 0)
        {
            myTarget.Kill();

            FinishJob();
        }
    }
}
