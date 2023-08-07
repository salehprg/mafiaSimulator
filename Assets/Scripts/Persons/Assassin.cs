using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Assassin : Person
{
    public float killTime;
    public GameObject killAnim;
    
    public override void ReachTarget(ITargetable _target)
    {
        waitTime = Time.time + killTime;
    }

    public override List<ITargetable> GetMyTargets()
    {
        var _targets = targets;
        _targets = _targets.Where(x => x != ((ITargetable)this)
                                    && ((Person)x).personStatus != PersonStatus.Prison
                                    && ((Person)x).personStatus != PersonStatus.Catched
                                    && ((Person)x).personStatus != PersonStatus.Dead).ToList();

        return _targets;
    }

    public override void DoingJob(ITargetable target)
    {
        if (waitTime - Time.time < 0)
        {
            (target as Person).Kill();

            FinishJob();
        }
    }

    public override void FinishJobPlayAnim(ITargetable target)
    {
        var tmp = Instantiate(killAnim, (target as MonoBehaviour).transform.position, new Quaternion());
        tmp.transform.SetParent((target as MonoBehaviour).transform);
        Destroy(tmp, 1.5f);
    }
}
